using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBoost : SpellBoost
{
    public HomingBoost()
    {
        costModifier = 0.2f;
        spellType = SpellType.Chaos;
        spellName = "Mage-to-Mage missile";
        description = "Some of your bullets now are homing. Taking this skill again will increase the chances.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsHoming = true;
        spell.ChanceToHomingMissile += 10f;
        spell.ChangeCostModifier(costModifier);
    }
}
