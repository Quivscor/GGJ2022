using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerResourceModifierBoost : SpellBoost
{
    public LowerResourceModifierBoost()
    {
        costModifier = -0.3f;
        description = "Lowers spell cost by 30%. This means that generating opposite " +
            "resource will be also slower.";

    }
    public override void ProcessSpellBoost(Spell spell)
    {
        spell.ChangeCostModifier(costModifier);
    }
}
