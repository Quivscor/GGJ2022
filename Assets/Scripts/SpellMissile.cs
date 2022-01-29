using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	private GameObject parent = null;
	private float damageToApply = 0.0f;

	public void Initialize(GameObject parent, float damageToApply)
	{
		this.parent = parent;
		this.damageToApply = damageToApply;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.gameObject == parent)
			return;

		if (other.transform.root.TryGetComponent(out CharacterStats characterStats))
		{
			characterStats.DealDamge(damageToApply);
		}

		Destroy(this.gameObject);
	}
}
