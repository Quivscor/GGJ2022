using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireRateSpellBoost", menuName = "ScriptableObjects/FireRate")]
public class FireRateSpellBoost : SpellBoostScriptable
{
    public float fireRateBonus = 2;
    public float damageModifier = 0.1f;

    public override void ProcessSpellBoost(Spell spell)
    {
        spell.FireRate += fireRateBonus;
        spell.Damage -= spell.Damage * damageModifier;
    }

   
}
