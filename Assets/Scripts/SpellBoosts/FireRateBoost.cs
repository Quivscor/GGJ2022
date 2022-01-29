using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBoost : SpellBoost
{
    public FireRateBoost()
    {
        spellType = SpellType.Any;
        costModifier = 0.0f;
        description = "Adds 1 attack per second. Damage is lower by 5%.";
    }
    public override void ProcessSpellBoost(Spell spell)
    {
        spell.FireRate += 1;
        spell.Damage *= 0.95f;
    }


}
