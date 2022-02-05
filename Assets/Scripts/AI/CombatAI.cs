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

        if (aiType == SpenderType.CONSERVER)
        {
            //define stronger spell
            if (isPrimaryStronger)
            {
                if (stats.HaveEnoughResource(wand.LeftSpell.currentData.resourceCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
            else
            {
                if (stats.HaveEnoughResource(wand.RightSpell.currentData.resourceCost, SpellType.Left))
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
                if (stats.HaveEnoughResource(wand.LeftSpell.currentData.resourceCost, SpellType.Left))
                    wand.ProcessSpell(SpellType.Left);
                else
                    wand.ProcessSpell(SpellType.Right);
            }
            else
            {
                //cast right
                if (stats.HaveEnoughResource(wand.RightSpell.currentData.resourceCost, SpellType.Left))
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