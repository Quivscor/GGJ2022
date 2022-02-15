using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitBoost", menuName = "ScriptableObjects/OrbitSpellBoost")]
public class OrbitSpellBoost : BasicSpellBoost
{
	[SerializeField] private float orbitRadius = 3f;
	[SerializeField] private float orbitPushoutForce = 5f;
	[SerializeField] private float slerpTime = .06f;
	private Vector3 velocityRef;
	[SerializeField] private float smoothDampTime = 0.05f;

	public override void ProcessSpellBoost(SpellData spell, CharacterStats stats)
	{
		base.ProcessSpellBoost(spell, stats);

		spell.MissileUpdated += OnMissileUpdated;
	}

	private void OnMissileUpdated(SpellMissile missile, SpellMissileEventData data)
	{
		Vector3 newVelocity = missile.Rigidbody.velocity;
		Vector3 vectorToParent = missile.GetParentTransform().position - missile.transform.position;
		if (vectorToParent.magnitude < orbitRadius)
			newVelocity += -vectorToParent * orbitPushoutForce;

		Vector3 dir = Vector3.Slerp(newVelocity.normalized, vectorToParent.normalized, slerpTime);
		//don't touch Y
		dir.y = missile.Rigidbody.velocity.y;

		newVelocity = dir.normalized * data.spell.spellSpeed;

		//smooth a bit
		Vector3.SmoothDamp(missile.Rigidbody.velocity, newVelocity, ref velocityRef, smoothDampTime);

		missile.Rigidbody.velocity = newVelocity;
		missile.RecalculateMissileVectors();
	}
}