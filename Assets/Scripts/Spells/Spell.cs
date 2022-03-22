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
    public SpellData Data => baseData;

    private CharacterStats characterStats;

    [SerializeField] private List<SpellBoostScriptable> spellBoosts;
    public List<SpellBoostScriptable> SpellBoosts => spellBoosts;

    public Action<SpellType, float> FireRateChanged;

    //Should be reserved only for cases when spelldata cannot be added in prefab, 
    //such as creating a turret that clones users spell data
    public void OverrideBaseData(SpellData newData)
    {
        baseData = newData.Clone();
    }

    public SpellData GetBaseData()
    {
        return baseData.Clone();
    }

    public bool IsHasBaseData()
    {
        return baseData != null;
    }

    //Should be reserved only for cases when spelldata cannot be added in prefab, 
    //such as creating a turret that clones users spell data
    public void OverrideSpellBoostList(List<SpellBoostScriptable> newBoosts)
    {
        spellBoosts = new List<SpellBoostScriptable>(newBoosts);
        RecalculateSpellBoosts(characterStats);
    }

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

        baseData.SetupStatDictionary();
        baseData.ClearEventSubscriptions();

        foreach(SpellBoostScriptable boost in spellBoosts)
        {
            boost.ProcessSpellBoost(baseData, stats);
        }

        GameStat resourceCost = baseData.GetStat(SpellStatType.Cost);
        resourceCost.UpdateBaseValue(resourceCost.InitValue * baseData.GetStat(SpellStatType.CostModifier).Max);

        //update timer in SpellcastingController
        FireRateChanged?.Invoke(spellType, baseData.GetStat(SpellStatType.Firerate).Max);
    }
}
