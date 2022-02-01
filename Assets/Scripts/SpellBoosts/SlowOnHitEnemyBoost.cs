using System;
using System.Collections;
using UnityEngine;

public class SlowOnHitEnemyBoost : SpellBoost
{
	private float chanceToSlowEnemyPerHit = 0.20f; //20%

    public SlowOnHitEnemyBoost()
    {
		spellType = SpellType.Harmony;
		costModifier = 0.15f;
		spellName = "Slippery Snail";
		description = "Adds a 15 % chance that the missile will slow target. Adds +5% damage";
	}
	public override void ProcessSpellBoost(Spell spell)
	{
		
		spell.MissileHitCharacter += SlowOnHitTarget;
		spell.Damage += spell.Damage * 0.05f;
		spell.ChangeCostModifier(costModifier);
	}

	private void SlowOnHitTarget(CharacterStats parent, CharacterStats target, float damage)
	{
		if (UnityEngine.Random.Range(0.0f, 1.0f) < chanceToSlowEnemyPerHit)
			target.GetComponent<CharacterMovement>()?.AddSlowEffect();
	}
}