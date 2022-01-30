using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateToxicAreaOnHitBoost : SpellBoost
{
	private float toxicAreaLifetime = 5f;
	private int spawnEveryXShot = 5;
	private int currentShots = 0;
	private float chanceToCreateArea = 0f;
	private float chanceToCreateAreaStep = 10f;


	private ToxicArea toxicAreaPrefab = null;

	public CreateToxicAreaOnHitBoost()
	{
		costModifier = 0.1f;
		spellType = SpellType.Chaos;
		spellName = "Forbidden Circle";
		description = "10% chance to create dangerous zone. Taking this skill again increases the chances by 10%.";
		toxicAreaPrefab = Resources.Load<ToxicArea>("ToxicArea");
	}

	public override void ProcessSpellBoost(Spell spell)
	{
		spell.MissileHitAnything += CreateToxicAreaInRange;
		spell.ChangeCostModifier(costModifier);
		chanceToCreateArea += chanceToCreateAreaStep;
	}

	private void CreateToxicAreaInRange(Transform missilePosition, CharacterStats owner)
	{
		if (Random.Range(0,100) < chanceToCreateArea)
		{
			currentShots = 0;

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
