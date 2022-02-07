using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ToxicAreaSpellBoost", menuName = "ScriptableObjects/ToxicAreaSpellBoost")]
public class ToxicAreaSpellBoost : SpellBoostScriptable
{
	public float toxicAreaLifetime = 5f;
	public int spawnEveryXShot = 5;
	public int currentShots = 0;
	public float chanceToCreateArea = 0f;
	public float chanceToCreateAreaStep = 10f;

	public ToxicArea toxicAreaPrefab = null;

	public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
	{
		spell.MissileHitAnything += CreateToxicAreaInRange;
		spell.costModifier += resourceModifier;
		chanceToCreateArea += chanceToCreateAreaStep;
		spell.chanceToCreateForbiddenArea = chanceToCreateArea;
	}

	private void CreateToxicAreaInRange(SpellMissileEventData data)
	{
		if (Random.Range(0, 100) < chanceToCreateArea)
		{
			currentShots = 0;

			if (toxicAreaPrefab == null)
			{
				Debug.LogError("Toxic area prefab reference is null!");
				return;
			}

			Vector3 spawnPosition = data.ownerTransform.position;
			spawnPosition.y = 0.0f;

			var spawnedArea = MonoBehaviour.Instantiate(toxicAreaPrefab, spawnPosition, Quaternion.identity);
			MonoBehaviour.Destroy(spawnedArea.gameObject, toxicAreaLifetime);
		}
	}
}
