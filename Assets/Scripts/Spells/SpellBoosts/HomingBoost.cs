using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBoost : SpellBoost
{
    public HomingBoost()
    {
        costModifier = 0.15f;
        spellType = SpellType.Right;
        spellName = "Mage-to-Mage missile";
        description = "Adds a 10% chance that the missile will be a homing missile.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsHoming = true;
        spell.ChanceToHomingMissile += 10f;
        spell.ChangeCostModifier(costModifier);
    }
}
