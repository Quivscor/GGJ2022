using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class SpellcastingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpellMissile bullet;

    [Header("Data")]
    //[SerializeField]
    //private Spell leftSpell;
    //public Spell LeftSpell => leftSpell;
    //[SerializeField]
    //private Spell rightSpell;
    //public Spell RightSpell => rightSpell;
    [SerializeField] private string[] spellMissileIgnoreTagList;
    public string[] SpellIgnoreTagList => spellMissileIgnoreTagList;
    [SerializeField] private string[] spellMissileBounceTagList;
    public string[] SpellBounceTagList => spellMissileBounceTagList;

    //[SerializeField] private Animator animator = null;
    [Space(10)]
    [SerializeField] private Transform bulletOrigin = null;
    private CharacterStats characterStats;
    private CharacterMovement characterMovement;

    private SpellType lastSpellType = SpellType.Left;
    #region Events
    public event Action<SpellType> SpellTypeChanged = null;
    public event Action<SpellcastEventData> SpellProcessed = null;
    public event Action<SpellcastEventData> SpellFired = null;
    public event Action ProjectileFired = null;
    #endregion

    #region Timers
    private Timer leftSpellCooldownTimer;
    private bool isLeftSpellReady = true;
    private Timer rightSpellCooldownTimer;
    private bool isRightSpellReady = true;
    private Timer leftSpellLastUseTimer;
    private bool hasFiredLeftSpell = false;
    private Timer rightSpellLastUseTimer;
    private bool hasFiredRightSpell = false;
    #endregion

    [SerializeField] float leftSpellLastTime = .5f;
    [SerializeField] float rightSpellLastTime = .5f;

    private void Awake()
	{
        characterStats = GetComponent<CharacterStats>();
        characterMovement = GetComponent<CharacterMovement>();

        //allows for setting boosts straight from inspector
        if(characterStats.LeftSpell.IsHasBaseData())
            characterStats.LeftSpell.RecalculateSpellBoosts(characterStats);
        if(characterStats.RightSpell.IsHasBaseData())
            characterStats.RightSpell.RecalculateSpellBoosts(characterStats);

        if (characterMovement != null)
            characterMovement.DashProcessed += ProcessDashSpellEffect;
    }

    private void Start()
    {
        leftSpellLastUseTimer = new Timer(leftSpellLastTime, () => hasFiredLeftSpell = false);
        rightSpellLastUseTimer = new Timer(rightSpellLastTime, () => hasFiredRightSpell = false);

        //if there's a boost that reduces time for these, they have to be recreated every time this value would be changed
        leftSpellCooldownTimer = new Timer(1 / characterStats.LeftSpell.Data.GetStat(SpellStatType.Firerate).Max, () => isLeftSpellReady = true);
        rightSpellCooldownTimer = new Timer(1 / characterStats.RightSpell.Data.GetStat(SpellStatType.Firerate).Max, () => isRightSpellReady = true);

        //subscribe to events that remake those timers when firerate gets a boost or a buff
        characterStats.LeftSpell.Data.GetStat(SpellStatType.Firerate).AnyValueChanged += 
            () => leftSpellCooldownTimer = new Timer(1 / characterStats.LeftSpell.Data.GetStat(SpellStatType.Firerate).Max
            , () => isLeftSpellReady = true);
        characterStats.RightSpell.Data.GetStat(SpellStatType.Firerate).AnyValueChanged +=
            () => rightSpellCooldownTimer = new Timer(1 / characterStats.RightSpell.Data.GetStat(SpellStatType.Firerate).Max
            , () => isLeftSpellReady = true);
    }

    private void Update()
    {
        float time = Time.deltaTime;
        //timers for switching between left and right spell
        leftSpellLastUseTimer.Update(time);
        rightSpellLastUseTimer.Update(time);

        //timers for cooldowns on left and right spell
        leftSpellCooldownTimer.Update(time);
        rightSpellCooldownTimer.Update(time);
    }

    public void OverrideSpellTagLists(string[] ignoreList, string[] bounceList)
    {
        spellMissileIgnoreTagList = ignoreList;
        spellMissileBounceTagList = bounceList;
    }

	public void ProcessSpell(SpellType type)
	{
        if (type == SpellType.Left && !hasFiredRightSpell)
        {
            float leftSpellCost = characterStats.LeftSpell.Data.GetStat(SpellStatType.Cost).Max;
            if (characterStats.HaveEnoughResource(leftSpellCost, SpellType.Left))
            {
                leftSpellLastUseTimer.RestartTimer();
                hasFiredLeftSpell = true;
            }
            
            if(isLeftSpellReady && characterStats.HaveEnoughResource(leftSpellCost, SpellType.Left))
            {
                Fire(characterStats.LeftSpell);
                isLeftSpellReady = false;
                leftSpellCooldownTimer.RestartTimer();
            }
        }
        else if (type == SpellType.Right && !hasFiredLeftSpell)
        {
            float rightSpellCost = characterStats.RightSpell.Data.GetStat(SpellStatType.Cost).Max;
            if (characterStats.HaveEnoughResource(rightSpellCost, SpellType.Right))
            {
                rightSpellLastUseTimer.RestartTimer();
                hasFiredRightSpell = true;
            }

            if (isRightSpellReady && characterStats.HaveEnoughResource(rightSpellCost, SpellType.Right))
            {
                Fire(characterStats.RightSpell);
                isRightSpellReady = false;
                rightSpellCooldownTimer.RestartTimer();
            }
        }

        if (lastSpellType != type)
        {
            lastSpellType = type;
            SpellTypeChanged?.Invoke(lastSpellType);
        }

        SpellProcessed?.Invoke(new SpellcastEventData(type));
    }

    private void Fire(Spell spell)
    {
        characterStats.SpendResource(spell.Data.GetStat(SpellStatType.Cost).Max, spell.spellType);

        int bulletCount = (int)spell.Data.GetStat(SpellStatType.BulletCount).Max;
        for (int i = 0; i < bulletCount; i++)
        {
            var firedProjectile = GameObject.Instantiate(bullet, bulletOrigin.transform.position, Quaternion.identity);
            //first subscribe
            firedProjectile.SubscribeToEvents(spell);
            //then call initialize (event MissileInitialized)
            firedProjectile.Initialize(characterStats, spell, CalculateBulletDirection(spell, i));
            firedProjectile.SetIgnoreTagList(spellMissileIgnoreTagList);
            firedProjectile.SetBounceTagList(spellMissileBounceTagList);
            ProjectileFired?.Invoke();
        }

        SpellFired?.Invoke(new SpellcastEventData(spell.spellType));
    }

    private void ProcessDashSpellEffect(SpellcastEventData data)
    {
        characterStats.LeftSpell.Data.DashProcessed?.Invoke(new SpellMissileEventData(this.transform));
        characterStats.RightSpell.Data.DashProcessed?.Invoke(new SpellMissileEventData(this.transform));
    }

    private Vector3 CalculateBulletDirection(Spell spell, int index)
    {
        if (index == 0)
            return bulletOrigin.transform.forward;

        GameStat shotAngle = spell.Data.GetStat(SpellStatType.ShotAngle);
        float angle = shotAngle.Max + ((index - 1) / 2 * shotAngle.Max);
        if (index % 2 != 0)
            return Vector3.RotateTowards(bulletOrigin.transform.forward, bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
        else
            return Vector3.RotateTowards(bulletOrigin.transform.forward, -bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
    }
}
