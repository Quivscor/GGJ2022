using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	private CharacterStats parent = null;
	private float damageToApply = 0.0f;

	public event Action<CharacterStats, CharacterStats, float> HitCharacter = null;

	public void Initialize(CharacterStats parent, float damageToApply, Action<CharacterStats, CharacterStats, float> missileHitCharacter)
	{
		this.parent = parent;
		this.damageToApply = damageToApply;
		HitCharacter = missileHitCharacter;
	}

	public Transform GetParentTransform()
    {
		return parent.transform;
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.CompareTag("Bullet"))
			return; 

		if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
		{
			if (targetCharacterStats == parent)
				return;

			targetCharacterStats.DealDamge(damageToApply);
			HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);
		}

		Destroy(this.gameObject);
	}
}
