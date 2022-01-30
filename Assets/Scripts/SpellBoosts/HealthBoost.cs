using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : SpellBoost
{
    public HealthBoost()
    {
        spellType = SpellType.Any;
        costModifier = 0.0f;
        spellName = "Healthy";
        description = "Increases health.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.GetComponent<CharacterStats>().maxHealth += 20f;
    }
}
