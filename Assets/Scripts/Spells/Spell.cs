using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Spell
{
    public SpellType spellType;
    [SerializeField] private SpellData baseData;
    [HideInInspector] public SpellData currentData;

    private CharacterStats characterStats;
    
    //public AudioSource audioSourceSpell;

    [SerializeField] private List<SpellBoostScriptable> spellBoosts;
    public List<SpellBoostScriptable> SpellBoosts => spellBoosts;

    public Action<SpellType, float> FireRateChanged;

    public void AddSpellBoost(SpellBoostScriptable boost)
    {
        SpellBoosts.Add(boost);
        RecalculateSpellBoosts(characterStats);
    }

    public void RemoveSpellBoost(SpellBoostScriptable boost)
    {
        SpellBoosts.Remove(boost);
        RecalculateSpellBoosts(characterStats);
    }

    public void RecalculateSpellBoosts(CharacterStats stats)
    {
        //assign stats through SpellcastingController.Awake call
        if (characterStats == null)
            characterStats = stats;

        spellBoosts.Sort((x, y) =>
        {
            if (x.sortingPriority > y.sortingPriority)
                return 1;
            else if (x.sortingPriority == y.sortingPriority)
                return 0;
            else
                return -1;
        });

        currentData = baseData.Clone();

        foreach(SpellBoostScriptable boost in spellBoosts)
        {
            boost.ProcessSpellBoost(currentData, stats);
        }

        currentData.resourceCost *= currentData.costModifier;

        //update timer in SpellcastingController
        FireRateChanged?.Invoke(spellType, currentData.fireRate);
    }
}
