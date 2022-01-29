using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfBulletsBoost : SpellBoost
{
    public NumberOfBulletsBoost()
    {
        costModifier = 0.1f;
        description = "Adds one attack to cast";
    }
    public override void ProcessSpellBoost(Spell spell)
    {
        spell.NumberOfBullets++;
        spell.ChangeCostModifier(costModifier);
    }
}