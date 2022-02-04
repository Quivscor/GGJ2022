using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBoosts : SpellBoost
{
    public BouncingBoosts()
    {
        costModifier = 0.1f;
        spellType = SpellType.Right;
        spellName = "Ping Pong";
        description = "Adds one bullet bounce. They deal additional +10% damage.";
    }

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.NumberOfBounces++;
        spell.Damage += spell.Damage * 0.1f;
        spell.ChangeCostModifier(costModifier);
    }
}
