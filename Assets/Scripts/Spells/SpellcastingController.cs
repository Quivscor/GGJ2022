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
    [SerializeField]
    private Spell leftSpell;
    public Spell LeftSpell => leftSpell;
    [SerializeField]
    private Spell rightSpell;
    public Spell RightSpell => rightSpell;
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
        if(leftSpell.IsHasBaseData())
            leftSpell.RecalculateSpellBoosts(characterStats);
        if(rightSpell.IsHasBaseData())
            rightSpell.RecalculateSpellBoosts(characterStats);

        if(characterMovement != null)
            characterMovement.DashProcessed += ProcessDashSpellEffect;
    }

    private void Start()
    {
        leftSpellLastUseTimer = new Timer(leftSpellLastTime, () => hasFiredLeftSpell = false);
        rightSpellLastUseTimer = new Timer(rightSpellLastTime, () => hasFiredRightSpell = false);

        //if there's a boost that reduces time for these, they have to be recreated every time this value would be changed
        leftSpellCooldownTimer = new Timer(1 / leftSpell.currentData.fireRate, () => isLeftSpellReady = true);
        rightSpellCooldownTimer = new Timer(1 / rightSpell.currentData.fireRate, () => isRightSpellReady = true);
        leftSpell.FireRateChanged += UpdateSpellCooldown;
        rightSpell.FireRateChanged += UpdateSpellCooldown;
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
            if (characterStats.HaveEnoughResource(leftSpell.currentData.resourceCost, SpellType.Left))
            {
                leftSpellLastUseTimer.RestartTimer();
                hasFiredLeftSpell = true;
            }
            
            if(isLeftSpellReady && characterStats.HaveEnoughResource(leftSpell.currentData.resourceCost, SpellType.Left))
            {
                Fire(leftSpell);
                isLeftSpellReady = false;
                leftSpellCooldownTimer.RestartTimer();
            }
        }
        else if (type == SpellType.Right && !hasFiredLeftSpell)
        {
            if (characterStats.HaveEnoughResource(rightSpell.currentData.resourceCost, SpellType.Right))
            {
                rightSpellLastUseTimer.RestartTimer();
                hasFiredRightSpell = true;
            }

            if (isRightSpellReady && characterStats.HaveEnoughResource(rightSpell.currentData.resourceCost, SpellType.Right))
            {
                Fire(rightSpell);
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
        characterStats.SpendResource(spell.currentData.resourceCost, spell.spellType);

        for(int i = 0; i < spell.currentData.numberOfBullets; i++)
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
        leftSpell.currentData.DashProcessed?.Invoke(new SpellMissileEventData(this.transform));
        rightSpell.currentData.DashProcessed?.Invoke(new SpellMissileEventData(this.transform));
    }

    private Vector3 CalculateBulletDirection(Spell spell, int index)
    {
        if (index == 0)
            return bulletOrigin.transform.forward;

        float angle = spell.currentData.angleBetweenShots + ((index - 1) / 2 * spell.currentData.angleBetweenShots);
        if (index % 2 != 0)
            return Vector3.RotateTowards(bulletOrigin.transform.forward, bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
        else
            return Vector3.RotateTowards(bulletOrigin.transform.forward, -bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
    }

    private void UpdateSpellCooldown(SpellType spell, float value)
    {
        if (spell == SpellType.Left)
            leftSpellCooldownTimer = new Timer(1 / value, () => isLeftSpellReady = true);
        if(spell == SpellType.Right)
            rightSpellCooldownTimer = new Timer(1 / value, () => isRightSpellReady = true);
    }
}
