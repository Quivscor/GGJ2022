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
		float lifesteal = data.spell.GetStat(SpellStatType.Lifesteal).Max;
		if (lifesteal > 0 && !data.targetStats.isDead)
		{
			data.ownerStats?.Heal(data.spell.GetStat(SpellStatType.Damage).Max * lifesteal);
			//weird but ok
			data.ownerStats?.PlayHealSound();
			GameObject spawnedParticles = null;

			if(data.ownerStats != null)
				spawnedParticles = MonoBehaviour.Instantiate(Resources.Load<GameObject>("HealingParticles"), data.ownerTransform);

			MonoBehaviour.Destroy(spawnedParticles, 1.5f);
		}
	}
}
