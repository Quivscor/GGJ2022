using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{
    public Transform activeTarget;

    private SpellcastingController wand;
    private CharacterRotator rotator;
    private CharacterStats stats;

    private bool isPrimaryStronger;

    [SerializeField] SpenderType aiType;

    private void Awake()
    {
        wand = GetComponent<SpellcastingController>();
        rotator = GetComponentInChildren<CharacterRotator>();
        stats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        isPrimaryStronger = stats.HarmonyLevel > stats.ChaosLevel ? true : false;
    }

    private void Update()
    {
        if (MatchController.Instance.State != MatchState.ACTIVE || stats.isDead)
            return;

        if (activeTarget == null)
            return;

        rotator.LookAt(activeTarget.position);

        //if (aiType == SpenderType.CONSERVER)
        //{
        //    //define stronger spell
        //    if(isPrimaryStronger)
        //    {
        //        if (stats.HaveEnoughResource(wand.GetSpellCost(true, false), SpellType.Left))
        //            wand.ProcessSpell(true, false);
        //        else
        //            wand.ProcessSpell(false, true);
        //    }
        //    else
        //    {
        //        if (stats.HaveEnoughResource(wand.GetSpellCost(false, true), SpellType.Right))
        //            wand.ProcessSpell(false, true);
        //        else
        //            wand.ProcessSpell(true, false);
        //    }
        //}
        //else if(aiType == SpenderType.SPAMMER)
        //{
        //    if(Random.value > 0.5f)
        //    {
        //        //cast left
        //        if(stats.HaveEnoughResource(wand.GetSpellCost(true, false), SpellType.Left))
        //            wand.ProcessSpell(true, false);
        //        else
        //            wand.ProcessSpell(false, true);
        //    }
        //    else
        //    {
        //        //cast right
        //        if (stats.HaveEnoughResource(wand.GetSpellCost(false, true), SpellType.Right))
        //            wand.ProcessSpell(false, true);
        //        else
        //            wand.ProcessSpell(true, false);
        //    }
        //}        
    }
}

public enum SpenderType
{
    SPAMMER,
    CONSERVER
}