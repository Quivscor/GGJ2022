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
        description = "Your bullets now are homing";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsHoming = true;
        spell.HomingForce += .25f;
        spell.ChangeCostModifier(costModifier);
    }
}
