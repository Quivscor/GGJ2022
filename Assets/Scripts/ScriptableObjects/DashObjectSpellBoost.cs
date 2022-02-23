using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashObjectBoost", menuName = "ScriptableObjects/DashObjectSpellBoost")]
public class DashObjectSpellBoost : BasicSpellBoost
{
    public DashObject dashObject;

    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        base.ProcessSpellBoost(spell, stats);

        spell.DashProcessed += OnDashProcessed;
    }

    private void OnDashProcessed(SpellMissileEventData data)
    {
        var newObject = Instantiate(dashObject, data.ownerTransform.position, Quaternion.identity, null);
        newObject.Initialize(data.ownerTransform);
    }
}
