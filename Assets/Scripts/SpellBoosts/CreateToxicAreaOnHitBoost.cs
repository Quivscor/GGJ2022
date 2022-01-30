using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateToxicAreaOnHitBoost : SpellBoost
{
	private float toxicAreaLifetime = 5f;
	private int spawnEveryXShot = 16;
	private int currentShots = 0;
	private float chanceToCreateArea = 0f;
	private float chanceToCreateAreaStep = 5f;


	private ToxicArea toxicAreaPrefab = null;

	public CreateToxicAreaOnHitBoost()
	{
		costModifier = 0.1f;
		spellType = SpellType.Chaos;
		spellName = "Forbidden Circle";
		description = "Creates dangerous zone every 15th attack. Taking this skill again reduces the amount by 1.";
		toxicAreaPrefab = Resources.Load<ToxicArea>("ToxicArea");
	}

	public override void ProcessSpellBoost(Spell spell)
	{
		spell.MissileHitAnything += CreateToxicAreaInRange;
		spell.ChangeCostModifier(costModifier);
		spawnEveryXShot--;
	}

	private void CreateToxicAreaInRange(Transform missilePosition, CharacterStats owner)
	{
		if (currentShots == spawnEveryXShot)
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
		else
        {
			//Debug.Log(currentShots);
			currentShots++;
        }


	}
}
