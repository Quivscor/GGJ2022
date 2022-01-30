using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBoost : SpellBoost
{
    public FastBoost()
    {
        costModifier = 0.05f;
        spellType = SpellType.Any;
        spellName = "I am speed";
        description = "Your bullets are much faster.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.SpellSpeed += 2.5f;
        spell.ChangeCostModifier(costModifier);
    }
}
