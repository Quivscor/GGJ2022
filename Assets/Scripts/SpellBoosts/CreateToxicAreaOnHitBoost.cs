using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateToxicAreaOnHitBoost : SpellBoost
{
	private float toxicAreaLifetime = 5f;
	private float chanceToSlowEnemyPerHit = 0.05f; //5%

	private ToxicArea toxicAreaPrefab = null;

	public CreateToxicAreaOnHitBoost()
	{
		costModifier = 0.1f;
		spellType = SpellType.Chaos;
		spellName = "Forbidden Circle";
		description = "Creates zone that drains life";
		toxicAreaPrefab = Resources.Load<ToxicArea>("ToxicArea");
	}

	public override void ProcessSpellBoost(Spell spell)
	{


		spell.MissileHitAnything += CreateToxicAreaInRange;
		spell.ChangeCostModifier(costModifier);
	}

	private void CreateToxicAreaInRange(Transform missilePosition, CharacterStats owner)
	{
		if (UnityEngine.Random.Range(0.0f, 1.0f) < chanceToSlowEnemyPerHit)
		{
			if (toxicAreaPrefab == null)
			{
				Debug.LogError("Toxic area prefab reference is null!");
				return;
			}

			Vector3 spawnPosition = missilePosition.position;
			spawnPosition.y = 0.0f;

			var spawnedArea = MonoBehaviour.Instantiate(toxicAreaPrefab, spawnPosition, Quaternion.identity);
			MonoBehaviour.Destroy(spawnedArea.gameObject, toxicAreaLifetime);
		}
	}
}
