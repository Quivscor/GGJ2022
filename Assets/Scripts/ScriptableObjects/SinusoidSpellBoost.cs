using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavingBoost", menuName = "ScriptableObjects/WavingSpellBoost")]
public class SinusoidSpellBoost : BasicSpellBoost
{
    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        base.ProcessSpellBoost(spell, stats);

        spell.MissileUpdated += OnMissileUpdated;
    }

    private void OnMissileUpdated(SpellMissile missile, SpellMissileEventData data)
    {
        Vector3 newVelocity = missile.Rigidbody.velocity;
        float mag = newVelocity.magnitude;
        float sign = 0;

        //initial offset so we start from cos(time) = 0
        float time = missile.LifeTime + ((Mathf.PI) / 2) * data.spell.sinusoidPhase;

        //if cos is decreasing in current range
        if (time % ((2*Mathf.PI) * data.spell.sinusoidPhase) <= (Mathf.PI * data.spell.sinusoidPhase))
            sign = -1;
        else
            sign = 1;

        newVelocity += missile.Right * sign * data.spell.sinusoidForce;

        missile.Rigidbody.velocity = newVelocity.normalized * mag;
    }
}
