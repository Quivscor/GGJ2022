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
    
    //[SerializeField] private Animator animator = null;
    [Space(10)]
    [SerializeField] private Transform bulletOrigin = null;
    private CharacterStats characterStats;

    private SpellType lastSpellType = SpellType.Left;
    #region Events
    public event Action<SpellType> SpellTypeChanged = null;
    public event Action SpellProcessed = null;
    public event Action SpellFired = null;
    public event Action ProjectileFired = null;
    public event Action<CharacterStats, CharacterStats, float> ProjectileHitCharacter = null;
    public event Action<Transform, CharacterStats> ProjectileHitAnything = null;
    #endregion

    #region Timers
    private Timer leftSpellCooldownTimer;
    private bool isLeftSpellReady = true;
    private Timer rightSpellCooldownTimer;
    private bool isRightSpellReady = true;
    private Timer leftSpellLastUseTimer;
    private bool hasFiredLeftSpell = true;
    private Timer rightSpellLastUseTimer;
    private bool hasFiredRightSpell = true;
    #endregion

    [SerializeField] float leftSpellLastTime = .5f;
    [SerializeField] float rightSpellLastTime = .5f;

    private void Awake()
	{
        characterStats = GetComponent<CharacterStats>();

        //allows for setting boosts straight from inspector
        leftSpell.RecalculateSpellBoosts();
        rightSpell.RecalculateSpellBoosts();
    }

    private void Start()
    {
        leftSpellLastUseTimer = new Timer(leftSpellLastTime, () => hasFiredLeftSpell = true);
        rightSpellLastUseTimer = new Timer(rightSpellLastTime, () => hasFiredRightSpell = true);

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

	public void ProcessSpell(SpellType type)
	{
        if (type == SpellType.Left && hasFiredRightSpell)
        {
            if (characterStats.HaveEnoughResource(leftSpell.currentData.resourceCost, SpellType.Left))
            {
                leftSpellLastUseTimer.RestartTimer();
            }
            
            if(isLeftSpellReady && characterStats.HaveEnoughResource(leftSpell.currentData.resourceCost, SpellType.Left))
            {
                Fire(leftSpell);
                isLeftSpellReady = false;
                leftSpellCooldownTimer.RestartTimer();
            }
        }
        else if (type == SpellType.Right && hasFiredLeftSpell)
        {
            if (characterStats.HaveEnoughResource(rightSpell.currentData.resourceCost, SpellType.Right))
            {
                rightSpellLastUseTimer.RestartTimer();
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

        SpellProcessed?.Invoke();
        //ProcessAnimator(type);
    }

    private void Fire(Spell spell)
    {
        characterStats.SpendResource(spell.currentData.resourceCost, spell.spellType);

        //audio
        //leftSource.Play()

        for(int i = 0; i < spell.currentData.numberOfBullets; i++)
        {
            var firedProjectile = GameObject.Instantiate(bullet, bulletOrigin.transform.position, Quaternion.identity);
            firedProjectile.Initialize(this.transform, spell, CalculateBulletDirection(spell, i));
            firedProjectile.SubscribeToEvents(spell);
            ProjectileFired?.Invoke();
        }

        SpellFired?.Invoke();
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

    //   private void ProcessAnimator(SpellType type)
    //{
    //       bool isSpell = false;
    //       if (type != SpellType.None)
    //       {
    //           animator.SetLayerWeight(1, 1);
    //           isSpell = true;
    //       }
    //       else
    //           animator.SetLayerWeight(1, 0);

    //       animator?.SetBool("isAttacking", isSpell);
    //   }
}
