using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{
    public Transform activeTarget;

    private WandController wand;
    private CharacterRotator rotator;
    private CharacterStats stats;

    [SerializeField] SpenderType aiType;

    private void Awake()
    {
        wand = GetComponent<WandController>();
        rotator = GetComponent<CharacterRotator>();
        stats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (activeTarget == null)
            return;

        rotator.LookAt(activeTarget.position);

        if (aiType == SpenderType.CONSERVER)
        {
            //define stronger spell

            //if able, use stronger spell

            //else use weaker
        }
        else if(aiType == SpenderType.SPAMMER)
        {
            if(Random.value > 0.5f)
            {
                //cast left
                if(stats.HaveEnoughResource(wand.GetSpellCost(true, false), SpellType.Harmony))
                    wand.ProcessSpell(true, false);
                else
                    wand.ProcessSpell(false, true);
            }
            else
            {
                //cast right
                if (stats.HaveEnoughResource(wand.GetSpellCost(false, true), SpellType.Chaos))
                    wand.ProcessSpell(false, true);
                else
                    wand.ProcessSpell(true, false);
            }
        }        
    }
}

public enum SpenderType
{
    SPAMMER,
    CONSERVER
}