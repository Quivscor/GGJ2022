using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FasterBulletsSpellBoost", menuName = "ScriptableObjects/FasterBullets")]
public class FasterBulletsSpellBoost : SpellBoostScriptable
{
    public float spellSpeedBonus = 3.5f;
    public float damageModifier = 0.1f;

    public override void ProcessSpellBoost(SpellData spell)
    {
        spell.spellSpeed += spellSpeedBonus;
        spell.damage += spell.damage * damageModifier;
        spell.costModifier += resourceModifier;
    }
}
