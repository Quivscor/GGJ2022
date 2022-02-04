using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedingBoost : SpellBoost
{
    public SpeedingBoost()
    {
        costModifier = 0.1f;
        spellType = SpellType.Any;
        spellName = "Speeeeeeeed Boost";
        description = "";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        
        spell.ChangeCostModifier(costModifier);
    }
}
