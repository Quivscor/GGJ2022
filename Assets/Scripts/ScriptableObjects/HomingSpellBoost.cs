using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingBoost", menuName = "ScriptableObjects/HomingSpellBoost")]
public class HomingSpellBoost : BasicSpellBoost
{
	[SerializeField] private float homingRange;
	[SerializeField] private float homingForce;

    public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
    {
        base.ProcessSpellBoost(spell, stats);

		spell.MissileUpdated += OnMissileUpdated;
    }

    private void OnMissileUpdated(SpellMissile missile, SpellMissileEventData data)
    {
		Vector3 newVelocity = missile.Rigidbody.velocity;
		//detect any character in area
		Collider[] cols = Physics.OverlapSphere(missile.transform.position, homingRange);

		foreach (Collider col in cols)
		{
			if (col.TryGetComponent(out CharacterStats stats))
			{
				//don't home on dead people
				if (stats.isDead)
					continue;
				//don't home on yourself
				if (stats == data.ownerStats)
					continue;

				Vector3 dir = Vector3.Slerp(newVelocity.normalized, (stats.transform.position - missile.transform.position).normalized, homingForce);
				//don't touch Y value
				dir.y = missile.Rigidbody.velocity.y;

				newVelocity = dir * data.spell.GetStat(SpellStatType.Speed).Max;
			}
		}
		//curve a little in that direction
		missile.Rigidbody.velocity = newVelocity;
		missile.RecalculateMissileVectors();
	}
}
