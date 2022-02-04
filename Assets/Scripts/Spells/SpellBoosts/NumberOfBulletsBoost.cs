using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfBulletsBoost : SpellBoost
{
    public NumberOfBulletsBoost()
    {
        costModifier = 0.1f;
        spellType = SpellType.Any;
        spellName = "Rain of bullets";
        description = "Adds one missile to cast.";
    }
    public override void ProcessSpellBoost(Spell spell)
    {
        spell.NumberOfBullets++;
        spell.ChangeCostModifier(costModifier);
    }
}
