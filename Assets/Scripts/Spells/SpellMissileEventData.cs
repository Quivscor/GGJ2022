using UnityEngine;

public class SpellMissileEventData
{
    public SpellMissileEventData() { }

    public SpellMissileEventData(Transform ownerTransform, CharacterStats ownerStats)
    {
        this.ownerTransform = ownerTransform;
        this.ownerStats = ownerStats;
    }
    public SpellMissileEventData(Transform targetTransform, CharacterStats targetStats, SpellData spell)
    {
        this.targetTransform = targetTransform;
        this.targetStats = targetStats;
        this.spell = spell;
    }
    public SpellMissileEventData(CharacterStats ownerStats, CharacterStats targetStats, SpellData spell)
    {
        this.ownerStats = ownerStats;
        this.targetStats = targetStats;
        this.spell = spell;
    }
    public SpellMissileEventData(Transform ownerTransform, CharacterStats ownerStats, CharacterStats targetStats, SpellData spell)
    {
        this.ownerTransform = ownerTransform;
        this.ownerStats = ownerStats;
        this.targetStats = targetStats;
        this.spell = spell;
    }
    public SpellMissileEventData(Transform ownerTransform, Transform targetTransform, CharacterStats ownerStats, CharacterStats targetStats, SpellData spell)
    {
        this.ownerTransform = ownerTransform;
        this.targetTransform = targetTransform;
        this.ownerStats = ownerStats;
        this.targetStats = targetStats;
        this.spell = spell;
    }

    public Transform ownerTransform;
    public CharacterStats ownerStats;
    public Transform targetTransform;
    public CharacterStats targetStats;
    public SpellData spell;
}