using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowthBoost", menuName = "ScriptableObjects/GrowthSpellBoost")]
public class GrowingSpellBoost : BasicSpellBoost
{
    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        base.ProcessSpellBoost(spell, stats);

        spell.MissileUpdated += OnMissileUpdated;
    }

    private void OnMissileUpdated(SpellMissile missile, SpellMissileEventData data)
    {
        if (missile.transform.localScale.magnitude < data.spell.maxScale)
            missile.transform.localScale = missile.transform.localScale + (Vector3.one * data.spell.growingForce);
    }
}
