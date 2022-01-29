using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	private CharacterStats parent = null;
	private float damageToApply = 0.0f;

	public event Action<CharacterStats, CharacterStats, float> HitCharacter = null;
	public event Action<Transform, CharacterStats> HitAnything = null;

	public void Initialize(CharacterStats parent, float damageToApply, Action<CharacterStats, CharacterStats, float> missileHitCharacter, Action<Transform, CharacterStats> missileHitAnything)
	{
		this.parent = parent;
		this.damageToApply = damageToApply;
		HitCharacter = missileHitCharacter;
		HitAnything = missileHitAnything;
	}

	public Transform GetParentTransform()
    {
		return parent.transform;
    }

	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (collision.transform.root.CompareTag("Bullet"))
	//		return;

	//	if (collision.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
	//	{
	//		if (targetCharacterStats == parent)
	//			return;

	//		targetCharacterStats.DealDamge(damageToApply);
	//		HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);
	//		HitAnything?.Invoke(this.transform, parent);
	//	}

	//	Destroy(this.gameObject);
	//}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.CompareTag("Bullet"))
			return;

		if (!other.isTrigger)
		{
			if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
			{
				if (targetCharacterStats == parent)
					return;

				targetCharacterStats.DealDamge(damageToApply);
				HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);
				HitAnything?.Invoke(this.transform, parent);

				if(other.transform.root.TryGetComponent(out TargetingAI targeting))
                {
					targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), damageToApply);
                }
			}

			Destroy(this.gameObject);
		}
	}
}
