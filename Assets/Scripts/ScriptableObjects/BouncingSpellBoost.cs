using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BouncingSpellBoost", menuName = "ScriptableObjects/Bouncing")]
public class BouncingSpellBoost : SpellBoostScriptable
{
    public override void ProcessSpellBoost(SpellData spell)
    {
        spell.numberOfBounces++;
        spell.damage += spell.damage * 0.1f;
        spell.costModifier += resourceModifier;
    }
}