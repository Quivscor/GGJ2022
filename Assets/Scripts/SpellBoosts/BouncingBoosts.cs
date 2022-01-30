using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBoosts : SpellBoost
{
    public BouncingBoosts()
    {
        costModifier = 0.07f;
        spellType = SpellType.Chaos;
        spellName = "Bouncer";
        description = "Your bullets are bouncing from surfaces and they deal +5% damage. Taking this skill again adds 1 more bounce.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.NumberOfBounces++;
        spell.Damage += spell.Damage * 0.05f;
        spell.ChangeCostModifier(costModifier);
    }
}
