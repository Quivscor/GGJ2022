using System;
using System.Collections;
using UnityEngine;

public class SlowOnHitEnemyBoost : SpellBoost
{
	private float chanceToSlowEnemyPerHit = 0.20f; //20%

    public SlowOnHitEnemyBoost()
    {
		spellType = SpellType.Harmony;
		costModifier = 0.2f;
		spellName = "Slow them!";
		description = "Has 20% chance for slowing enemy on impact";
	}
	public override void ProcessSpellBoost(Spell spell)
	{
		
		spell.MissileHitCharacter += SlowOnHitTarget;
		spell.ChangeCostModifier(costModifier);
	}

	private void SlowOnHitTarget(CharacterStats parent, CharacterStats target, float damage)
	{
		if (UnityEngine.Random.Range(0.0f, 1.0f) < chanceToSlowEnemyPerHit)
			target.GetComponent<CharacterMovement>()?.AddSlowEffect();
	}
}