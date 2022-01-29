using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	[SerializeField] private GameObject hitImpactParticles = null;
	[SerializeField] private float hitImpactParticlesLifetime = 1.0f;

	private CharacterStats parent = null;
	private float damageToApply = 0.0f;
	private int bouncesLeft = 1;
	private bool bounced = false;

	public event Action<CharacterStats, CharacterStats, float> HitCharacter = null;
	public event Action<Transform, CharacterStats> HitAnything = null;
	private Rigidbody rb;
	private Vector3 lastVelocity;
	public void Initialize(CharacterStats parent, float damageToApply,int bounces, Action<CharacterStats, CharacterStats, float> missileHitCharacter, Action<Transform, CharacterStats> missileHitAnything)
	{
		rb = GetComponent<Rigidbody>();
		this.parent = parent;
		this.damageToApply = damageToApply;
		HitCharacter = missileHitCharacter;
		HitAnything = missileHitAnything;
	}

	public Transform GetParentTransform()
    {
		return parent.transform;
    }

	private void Update()
	{
		lastVelocity = rb.velocity;
	}

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.CompareTag("Bullet"))
        {
			HitAnything?.Invoke(this.transform, parent);
			CreateHitImpact();
			Destroy(collision.transform.root.gameObject);
			Destroy(this.gameObject);
        }

		HitAnything?.Invoke(this.transform, parent);

		if (collision.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
        {
            if (targetCharacterStats == parent && !bounced)
                return;

			if (!collision.transform.CompareTag("Player"))
				damageToApply *= 0.5f;
            targetCharacterStats.DealDamge(damageToApply);
            HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);

			if (collision.transform.root.TryGetComponent(out TargetingAI targeting))
            {
                targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), damageToApply);
            }

			CreateHitImpact();
			Destroy(this.gameObject);
		}

		if (bouncesLeft > 0)
		{
			bounced = true;
			var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
			GetComponent<Rigidbody>().velocity = direction * Mathf.Max(lastVelocity.magnitude, 1f);
			bouncesLeft--;
		}
		else
		{
			CreateHitImpact();
			Destroy(this.gameObject);
		}
    }

	private void CreateHitImpact()
	{
		if (hitImpactParticles != null)
		{
			GameObject createdImpact = Instantiate(hitImpactParticles, this.transform.position, Quaternion.identity);
			Destroy(createdImpact, hitImpactParticlesLifetime);
		}
	}

	//   private void OnTriggerEnter(Collider other)
	//{
	//	if (other.transform.root.CompareTag("Bullet"))
	//		return;

	//	if (!other.isTrigger)
	//	{
	//		if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
	//		{
	//			if (targetCharacterStats == parent)
	//				return;

	//			targetCharacterStats.DealDamge(damageToApply);
	//			HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);
	//			HitAnything?.Invoke(this.transform, parent);

	//			if(other.transform.root.TryGetComponent(out TargetingAI targeting))
	//               {
	//				targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), damageToApply);
	//               }
	//		}

	//		Destroy(this.gameObject);
	//	}
	//}
}
