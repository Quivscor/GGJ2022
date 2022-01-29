using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnHitEnemyBoost : SpellBoost
{
	private float healingFactor = 0.25f;

    public HealOnHitEnemyBoost()
    {
		costModifier = 0.1f;
		description = "Every attack heals you by +25% of damage dealt";
	}
	public override void ProcessSpellBoost(Spell spell)
	{
		spell.MissileHitCharacter += HealOnHitTarget;
	}

	private void HealOnHitTarget(CharacterStats parent, CharacterStats target, float damage)
	{
		parent.Heal(damage * healingFactor);
	}
}
