using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : SpellBoost
{
    public HealthBoost()
    {
        spellType = SpellType.Any;
        costModifier = 0.0f;
        spellName = "Pump the muscles";
        description = "Increases health by 25 points and adds +10% damage.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.GetComponent<CharacterStats>().maxHealth += 25f;
        spell.Damage += spell.Damage * 0.1f;
    }
}
