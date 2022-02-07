using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealOnHitBoost", menuName = "ScriptableObjects/HealOnHitSpellBoost")]
public class HealOnHitSpellBoost : BasicSpellBoost
{
	public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
	{
		base.ProcessSpellBoost(spell, stats);

		spell.MissileHitCharacter += HealOnHit;
	}

	public void HealOnHit(SpellMissileEventData data)
    {
		if (data.spell.lifeSteal > 0 && !data.targetStats.isDead)
		{
			data.ownerStats.Heal(data.spell.damage * data.spell.lifeSteal);
			//weird but ok
			data.ownerStats.PlayHealSound();
			GameObject spawnedParticles = null;

			spawnedParticles = MonoBehaviour.Instantiate(Resources.Load<GameObject>("HealingParticles"), data.ownerTransform);

			MonoBehaviour.Destroy(spawnedParticles, 1.5f);
		}
	}
}
