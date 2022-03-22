using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashSpeedBoost", menuName = "ScriptableObjects/DashSpeedSpellBoost")]
public class DashSpeedSpellBoost : BasicSpellBoost
{
    private CharacterStats ownerStats;
    [SerializeField] private BuffDataScriptable buff;
    [SerializeField] private float buffDuration;

    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        base.ProcessSpellBoost(spell, stats);
        ownerStats = stats;

        spell.DashProcessed += OnDashProcessed;
    }

    private void OnDashProcessed(SpellMissileEventData data)
    {
        Buff b = new Buff(buffDuration, buff);
        ownerStats.ApplyBuff(b);
    }
}
