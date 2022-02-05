using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicSpellBoost", menuName = "ScriptableObjects/BasicSpellBoost")]

public class BasicSpellBoost : SpellBoostScriptable
{
    public override void ProcessSpellBoost(Spell spell)
    {
        ChangeStats(spell, spell.GetComponent<CharacterStats>());
    }

}
