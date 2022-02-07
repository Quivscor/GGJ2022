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
        // ladder of ifs
        for (int i = 0; i < statsToChange.Count; i++)
        {
            switch (statsToChange[i].statsType)
            {
                case StatsType.Health:
                    if (statsToChange[i].isMultiplicative)
                        characterStats.maxHealth += characterStats.maxHealth * statsToChange[i].value;
                    else
                        characterStats.maxHealth += statsToChange[i].value;
                    break;

                case StatsType.Resource:
                    if (statsToChange[i].isMultiplicative)
                        spell.costModifier += spell.costModifier * statsToChange[i].value;
                    else
                        spell.costModifier += statsToChange[i].value;
                    if (spell.costModifier < 0.2f)
                        spell.costModifier = 0.2f;
                    break;

                case StatsType.Damage:
                    if (statsToChange[i].isMultiplicative)
                        spell.damage += spell.damage * statsToChange[i].value;
                    else
                        spell.damage += statsToChange[i].value;
                    break;

                case StatsType.Firerate:
                    if (statsToChange[i].isMultiplicative)
                        spell.fireRate += spell.fireRate * statsToChange[i].value;
                    else
                        spell.fireRate += statsToChange[i].value;
                    break;

                case StatsType.MovementSpeed:
                    if(statsToChange[i].isMultiplicative)
                        characterStats.GetComponent<CharacterMovement>().MovementSpeed += characterStats.GetComponent<CharacterMovement>().MovementSpeed * statsToChange[i].value;
                    else
                        characterStats.GetComponent<CharacterMovement>().MovementSpeed += statsToChange[i].value;
                    break;

                case StatsType.BulletSpeed:
                    if (statsToChange[i].isMultiplicative)
                        spell.spellSpeed += spell.spellSpeed * statsToChange[i].value;
                    else
                        spell.spellSpeed += statsToChange[i].value;
                    break;

                case StatsType.NumberOfBullets:
                    if (statsToChange[i].isMultiplicative)
                        spell.numberOfBullets += spell.numberOfBullets * (int)statsToChange[i].value;
                    else 
                        spell.numberOfBullets += (int)statsToChange[i].value;
                    break;

                case StatsType.NumberOfBounces:
                    if (statsToChange[i].isMultiplicative)
                        spell.numberOfBounces += spell.numberOfBounces * (int)statsToChange[i].value;
                    else
                        spell.numberOfBounces += (int)statsToChange[i].value;
                    break;
                case StatsType.Lifesteal:
                    if (statsToChange[i].isMultiplicative)
                        spell.lifeSteal += spell.lifeSteal * (int)statsToChange[i].value;
                    else
                        spell.lifeSteal += (int)statsToChange[i].value;
                    break;
            }
        }
    }

    public abstract void ProcessSpellBoost(SpellData spell, CharacterStats stats);
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
    NumberOfBounces,
    Lifesteal,
}
