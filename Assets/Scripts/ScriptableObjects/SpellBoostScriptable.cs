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

    public void ChangeStats(Spell spell, CharacterStats characterStats)
    {
        // ladder of ifs
        for (int i = 0; i < statsToChange.Count; i++)
        {
            if(statsToChange[i].statsType == StatsType.Health)
            {
                if(statsToChange[i].isMultiplicative)
                {
                    characterStats.maxHealth += characterStats.maxHealth * statsToChange[i].value;

                }
                else
                {
                    characterStats.maxHealth += statsToChange[i].value;
                }

                continue;
            }

            if (statsToChange[i].statsType == StatsType.Resource)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.CostModifier += spell.CostModifier * statsToChange[i].value;
                }
                else
                {
                    spell.CostModifier += statsToChange[i].value;
                }

                if (spell.CostModifier < 0.2f)
                    spell.CostModifier = 0.2f;

                spell.RecalculateResourceCost();
                continue;
            }

            if (statsToChange[i].statsType == StatsType.Damage)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.Damage += spell.Damage * statsToChange[i].value;
                }
                else
                {
                    spell.Damage += statsToChange[i].value;
                }

                continue;
            }

            if (statsToChange[i].statsType == StatsType.Firerate)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.FireRate += spell.FireRate * statsToChange[i].value;
                }
                else
                {
                    spell.FireRate += statsToChange[i].value;
                }

                spell.RecalculateFireRate();
                continue;
            }

            if (statsToChange[i].statsType == StatsType.MovementSpeed)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    characterStats.GetComponent<CharacterMovement>().MovementSpeed += characterStats.GetComponent<CharacterMovement>().MovementSpeed * statsToChange[i].value;

                }
                else
                {
                    characterStats.GetComponent<CharacterMovement>().MovementSpeed += statsToChange[i].value;
                }

                continue;
            }

            if (statsToChange[i].statsType == StatsType.BulletSpeed)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.SpellSpeed += spell.SpellSpeed * statsToChange[i].value;
                }
                else
                {
                    spell.SpellSpeed += statsToChange[i].value;
                }

                continue;
            }

            if (statsToChange[i].statsType == StatsType.NumberOfBullets)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.NumberOfBullets += spell.NumberOfBullets * (int)statsToChange[i].value;
                }
                else
                {
                    spell.NumberOfBullets += (int)statsToChange[i].value;
                }

                continue;
            }

            if (statsToChange[i].statsType == StatsType.NumberOfBounces)
            {
                if (statsToChange[i].isMultiplicative)
                {
                    spell.NumberOfBounces += spell.NumberOfBounces * (int)statsToChange[i].value;
                }
                else
                {
                    spell.NumberOfBounces += (int)statsToChange[i].value;
                }

                continue;
            }

        }
    }

    public abstract void ProcessSpellBoost(Spell spell);
}

[System.Serializable]
public struct StatToChange
{
    public StatsType statsType;
    public float value;
    public bool isMultiplicative;
}

public enum StatsType
{
    Health,
    Resource,
    Damage,
    Firerate,
    MovementSpeed,
    BulletSpeed,
    NumberOfBullets,
    NumberOfBounces
}
