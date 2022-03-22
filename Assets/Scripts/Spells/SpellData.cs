using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell")]
public class SpellData : ScriptableObject
{
    public SpellcastMode castMode = SpellcastMode.UNDEFINED;
    [Header("Base stats")]
    [SerializeField] private List<StatToChange> initialStatValues;
    private Dictionary<SpellStatType, GameStat> _spellStatDictionary;

    public void SetupStatDictionary()
    {
        _spellStatDictionary = new Dictionary<SpellStatType, GameStat>();
        for(SpellStatType i = 0; i != SpellStatType.UNDEFINED; i++)
        {
            float inspectorStat = initialStatValues.Find((x) => x.SpellStatType == i).value;
            _spellStatDictionary.Add(i, new GameStat(inspectorStat));
        }
    }

    public GameStat GetStat(SpellStatType type)
    {
        return _spellStatDictionary[type];
    }

    public void UpdateBaseStat(SpellStatType type, float value, bool isMultiplicative = false)
    {
        if (isMultiplicative)
            _spellStatDictionary[type].UpdateBaseValue(_spellStatDictionary[type].InitValue * value);
        else
            _spellStatDictionary[type].UpdateBaseValue(value);
    }

    public void ClearEventSubscriptions()
    {
        MissileHitCharacter = null;
        MissileHitAnything = null;
        MissileUpdated = null;
        MissileInitialized = null;
        DashProcessed = null;
    }

    //TODO - add list of applied bonuses IDs to check if boost already exists

    public Action<SpellMissileEventData> MissileHitCharacter = null;
    public Action<SpellMissileEventData> MissileHitAnything = null;
    public Action<SpellMissile, SpellMissileEventData> MissileUpdated = null;
    public Action<SpellMissile, SpellMissileEventData> MissileInitialized = null;
    public Action<SpellMissileEventData> DashProcessed = null;
}