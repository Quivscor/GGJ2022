using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBoost : SpellBoost
{
    public HomingBoost()
    {
        costModifier = 0.1f;
        spellType = SpellType.Chaos;
        spellName = "";
        description = "";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.IsHoming = true;
        spell.HomingForce += .25f;
        spell.ChangeCostModifier(costModifier);
    }
}
