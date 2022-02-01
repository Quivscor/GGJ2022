using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBoost : SpellBoost
{
    public FireRateBoost()
    {
        spellType = SpellType.Any;
        costModifier = 0.0f;
        spellName = "Ratatata";
        description = "Increases fire rate by 2 (two more attack per second). Damage is lowered by 10%.";
    }
    public override void ProcessSpellBoost(Spell spell)
    {
        spell.FireRate += 2;
        spell.Damage -= spell.Damage * 0.1f;
        spell.RecalculateFireRate();
    }


}
