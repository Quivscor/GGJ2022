using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class CombatAI : MonoBehaviour
{
    public Transform activeTarget;

    private SpellcastingController wand;
    private CharacterRotator rotator;
    private CharacterStats stats;

    private bool isPrimaryStronger;

    [SerializeField] SpenderType aiType;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float rangeCheckFrequency = 1f;
    private Timer rangeCheckTimer;
    private bool _canCheckRange = true;
    private bool _canFire = false;

    private void Awake()
    {
        wand = GetComponent<SpellcastingController>();
        rotator = GetComponentInChildren<CharacterRotator>();
        stats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        isPrimaryStronger = stats.HarmonyLevel > stats.ChaosLevel ? true : false;

        rangeCheckTimer = new Timer(rangeCheckFrequency, () => _canCheckRange = true);
    }

    private void Update()
    {
        if (MatchController.Instance.State != MatchState.ACTIVE || stats.isDead)
            return;

        if (activeTarget == null)
            return;

        rangeCheckTimer.Update(Time.deltaTime);
        if(_canCheckRange)
        {
            if (Vector3.Distance(this.activeTarget.position, this.transform.position) < attackRange)
                _canFire = true;
            else
                _canFire = false;

            rangeCheckTimer.RestartTimer();
            _canCheckRange = false;
        }
        rotator.LookAt(activeTarget.position);

        if (!_canFire)
            return;

        float leftSpellCost = stats.LeftSpell.Data.GetStat(SpellStatType.Cost).Max;
        float rightSpellCost = stats.RightSpell.Data.GetStat(SpellStatType.Cost).Max;
        if (aiType == SpenderType.CONSERVER)
        {
            //define stronger spell
            if (isPrimaryStronger)
            {
                if (stats.HaveEnoughResource(leftSpellCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
            else
            {
                if (stats.HaveEnoughResource(rightSpellCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
        }
        else if (aiType == SpenderType.SPAMMER)
        {
            if (Random.value > 0.5f)
            {
                //cast left
                if (stats.HaveEnoughResource(leftSpellCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
            else
            {
                //cast right
                if (stats.HaveEnoughResource(rightSpellCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
        }
    }
}

public enum SpenderType
{
    SPAMMER,
    CONSERVER
}