using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowOnHitBoost", menuName = "ScriptableObjects/SlowOnHitSpellBoost")]
public class SlowOnHitBoost : BasicSpellBoost
{
	[SerializeField] private float debuffDuration = 3f;
	[SerializeField] private BuffDataScriptable debuffData;

	public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
	{
		base.ProcessSpellBoost(spell, stats);

		spell.MissileHitCharacter += SlowOnHit;
	}

	public void SlowOnHit(SpellMissileEventData data)
	{
		Buff slowDebuff = new Buff(debuffDuration, debuffData, false, true);
		data.targetStats.ApplyBuff(slowDebuff);
	}
}
