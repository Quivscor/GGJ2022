using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBoost : SpellBoost
{
    public FastBoost()
    {
        costModifier = 0.1f;
        spellType = SpellType.Any;
        spellName = "The speed of sound";
        description = "Your bullets are much faster and deal +10% more damage";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.SpellSpeed += 3.5f;
        spell.Damage += spell.Damage * 0.1f;
        spell.ChangeCostModifier(costModifier);
    }
}
