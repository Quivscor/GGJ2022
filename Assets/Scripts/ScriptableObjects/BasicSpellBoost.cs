using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicSpellBoost", menuName = "ScriptableObjects/BasicSpellBoost")]

public class BasicSpellBoost : SpellBoostScriptable
{
    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        ChangeStats(spell, stats);
    }
}
