using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnHitEnemyBoost : SpellBoost
{
	private float healingFactor = 0.66f;

	private GameObject healingEffectParticles = null;
	private GameObject healingEnemyEffectParticles = null;

    public HealOnHitEnemyBoost()
    {
		costModifier = 0.1f;
		spellType = SpellType.Any;
		spellName = "Hit & Heal";
		description = "Every attack heals you by +25% of damage dealt";

		healingEffectParticles = Resources.Load<GameObject>("HealingParticles");
		healingEnemyEffectParticles = Resources.Load<GameObject>("HealingParticlesEnemy");
	}

	public override void ProcessSpellBoost(Spell spell)
	{
		spell.MissileHitCharacter += HealOnHitTarget;
	}

	private void HealOnHitTarget(CharacterStats parent, CharacterStats target, float damage)
	{
		parent.Heal(damage * healingFactor);

		GameObject spawnedParticles = null;

		if (parent.TryGetComponent<PlayerInputController>(out PlayerInputController playerInputController))
		{
			spawnedParticles = MonoBehaviour.Instantiate(healingEffectParticles, parent.transform);
		}
		else
		{
			spawnedParticles = MonoBehaviour.Instantiate(healingEnemyEffectParticles, parent.transform);
		}

		MonoBehaviour.Destroy(spawnedParticles, 1.5f);
	}
}
