using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

[CreateAssetMenu(fileName = "SideEmitterBoost", menuName = "ScriptableObjects/SideEmitterSpellBoost")]
public class SideEmitterBoost : BasicSpellBoost
{
	public float timeBetweenFiring = .5f;
	public float projectileCount = 0;

	public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
	{
		base.ProcessSpellBoost(spell, stats);

		spell.MissileInitialized += OnMissileInitialized;
	}

	private void OnMissileInitialized(SpellMissile missile, SpellMissileEventData data)
    {
		missile.gameObject.AddComponent<ProjectileEmitter>().Initialize(timeBetweenFiring, projectileCount);
    }
}
