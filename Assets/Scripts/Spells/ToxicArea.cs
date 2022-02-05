using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{
	[SerializeField] private float damagePerSecond = 1.0f;

	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent(out CharacterStats characterStats))
		{
			characterStats.DealDamge(damagePerSecond * Time.fixedDeltaTime);
		}
	}
}
