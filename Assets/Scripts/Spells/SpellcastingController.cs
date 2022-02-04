using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class SpellcastingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Spell leftSpell;
    public Spell LeftSpell => leftSpell;
    [SerializeField]
    private Spell rightSpell;
    public Spell RightSpell => rightSpell;
    [SerializeField]
    private SpellMissile bullet;
    //[SerializeField] private Animator animator = null;
    [Space(10)]
    [SerializeField] private Transform bulletOrigin = null;
    private CharacterStats characterStats;

    private SpellType lastSpellType = SpellType.Left;
    #region Events
    public event Action<SpellType> SpellTypeChanged = null;
    public Action SpellProcessed = null;
    public Action SpellFired = null;
    public Action ProjectileFired = null;
    public event Action<CharacterStats, CharacterStats, float> ProjectileHitCharacter = null;
    public event Action<Transform, CharacterStats> ProjectileHitAnything = null;
    #endregion

    #region Timers
    private Timer leftSpellCooldownTimer;
    private bool isLeftSpellReady;
    private Timer rightSpellCooldownTimer;
    private bool isRightSpellReady;
    private Timer leftSpellLastUseTimer;
    private bool isCanFireLeftSpell;
    private Timer rightSpellLastUseTimer;
    private bool isCanFireRightSpell;
    #endregion

    private void Awake()
	{
        characterStats = GetComponent<CharacterStats>();

        leftSpellLastUseTimer = new Timer(leftSpellLastTime, () => isCanFireLeftSpell = true);
        rightSpellLastUseTimer = new Timer(rightSpellLastTime, () => isCanFireRightSpell = true);

        //if there's a boost that reduces time for these, they have to be recreated every time this value would be changed
        //leftSpellCooldownTimer = new Timer(, () => isLeftSpellReady = true);
        //rightSpellCooldownTimer = new Timer(, () => isRightSpellReady = true);
    }

    [SerializeField] float leftSpellLastTime = .5f;
    [SerializeField] float rightSpellLastTime = .5f;

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
        if (type == SpellType.Left && isCanFireRightSpell)
        {
            if (characterStats.HaveEnoughResource(leftSpell.resourceCost, SpellType.Left))
            {
                leftSpellLastUseTimer.RestartTimer();
            }
            
            if(isLeftSpellReady && characterStats.HaveEnoughResource(leftSpell.resourceCost, SpellType.Left))
            {
                Fire(leftSpell);
                isLeftSpellReady = false;
                leftSpellCooldownTimer.RestartTimer();
            }
        }
        else if (type == SpellType.Right && isCanFireLeftSpell)
        {
            if (characterStats.HaveEnoughResource(rightSpell.resourceCost, SpellType.Right))
            {
                rightSpellLastUseTimer.RestartTimer();
            }

            if (isRightSpellReady && characterStats.HaveEnoughResource(rightSpell.resourceCost, SpellType.Right))
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
        characterStats.SpendResource(spell.resourceCost, spell.spellType);

        //audio
        //leftSource.Play()

        for(int i = 0; i < spell.numberOfBullets; i++)
        {
            var firedProjectile = GameObject.Instantiate(bullet, bulletOrigin.transform.position, Quaternion.identity);
            firedProjectile.Initialize(this.transform, spell, CalculateBulletDirection(spell, i));
            firedProjectile.SubscribeToEvents(ProjectileHitCharacter, ProjectileHitAnything);
            ProjectileFired?.Invoke();
        }

        SpellFired?.Invoke();
    }

    private Vector3 CalculateBulletDirection(Spell spell, int index)
    {
        if (index == 0)
            return bulletOrigin.transform.forward;

        float angle = spell.angleBetweenShots + ((index - 1) / 2 * spell.angleBetweenShots);
        if (index % 2 != 0)
            return Vector3.RotateTowards(bulletOrigin.transform.forward, bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
        else
            return Vector3.RotateTowards(bulletOrigin.transform.forward, -bulletOrigin.transform.right, Mathf.Deg2Rad * angle, 0f).normalized;
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
