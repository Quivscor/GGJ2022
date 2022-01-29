using System;
using System.Collections;
using UnityEngine;

public class SlowOnHitEnemyBoost : SpellBoost
{
	private float chanceToSlowEnemyPerHit = 0.20f; //20%

	public override void ProcessSpellBoost(Spell spell)
	{
		spell.MissileHitCharacter += SlowOnHitTarget;
	}

	private void SlowOnHitTarget(CharacterStats parent, CharacterStats target, float damage)
	{
		if (UnityEngine.Random.Range(0.0f, 1.0f) < chanceToSlowEnemyPerHit)
			target.GetComponent<CharacterMovement>()?.AddSlowEffect();
	}
}