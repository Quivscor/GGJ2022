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
        description = "Your bullets are growing in the air. Additionally they deal +25% damage.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsGrowing = true;
        spell.GrowingForce += .0125f;
        spell.MaxScale += 1f;
        spell.ChangeCostModifier(costModifier);
        spell.Damage += spell.Damage * 0.25f;
    }
}
