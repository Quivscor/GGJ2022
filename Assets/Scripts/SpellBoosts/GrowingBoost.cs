using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBoost : SpellBoost
{
    public GrowingBoost()
    {
        costModifier = 0.3f;
        spellType = SpellType.Any;
        spellName = "Big Chungus";
        description = "Your bullets are now growing";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsGrowing = true;
        spell.GrowingForce += .0125f;
        spell.MaxScale = 3f;
        spell.ChangeCostModifier(costModifier);
        spell.Damage += 5;
    }
}
