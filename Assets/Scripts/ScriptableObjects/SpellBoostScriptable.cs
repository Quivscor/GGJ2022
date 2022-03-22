using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpellBoost", menuName = "ScriptableObjects")]
public abstract class SpellBoostScriptable : ScriptableObject
{
    public int id;
    public int sortingPriority;
    public string[] tags;
    public string spellName;
    [TextArea(10, 100)]
    public string description;
    public Image icon;
    public float resourceModifier;
    public SpellType spellType = SpellType.Any;
    public List<StatToChange> statsToChange = new List<StatToChange>();

    public void ChangeStats(SpellData spell, CharacterStats characterStats)
    {
        for (int i = 0; i < statsToChange.Count; i++)
        {
            if(statsToChange[i].CharacterStatType != CharacterStatType.UNDEFINED)
            {
                characterStats.UpdateBaseStat(statsToChange[i].CharacterStatType, statsToChange[i].value, statsToChange[i].isMultiplicative);
            }
            //not sure if anyone would want to use same value to affect different stats in one go, but this prevents it
            else if(statsToChange[i].SpellStatType != SpellStatType.UNDEFINED)
            {
                spell.UpdateBaseStat(statsToChange[i].SpellStatType, statsToChange[i].value, statsToChange[i].isMultiplicative);
            }
        }
    }

    public abstract void ProcessSpellBoost(SpellData spell, CharacterStats stats);
}

[System.Serializable]
public struct StatToChange
{
    public CharacterStatType CharacterStatType;
    public SpellStatType SpellStatType;
    public float value;
    public bool isMultiplicative;
}