using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissile : MonoBehaviour
{
	[SerializeField] private GameObject hitImpactParticles = null;
	[SerializeField] private float hitImpactParticlesLifetime = 1.0f;

	public CharacterStats parent = null;
	private float damageToApply = 0.0f;
	private int bouncesLeft = 0;
	private bool bounced = false;

	public event Action<CharacterStats, CharacterStats, float> HitCharacter = null;
	public event Action<Transform, CharacterStats> HitAnything = null;
	private Rigidbody rb;
	private Vector3 lastVelocity;

	[Header("Homing")]
	private bool isHoming;
	private float homingForce;
	[SerializeField] float enemyCheckFrequency;
	[SerializeField] float homingRange;
	private float enemyCheckCurrentTime;
	private bool isGrowing;
	private Vector3 growVector;
	private Vector3 maxScale;
	private float lifeSteal;

	public void Initialize(CharacterStats parent, float damageToApply, int bounces, Action<CharacterStats, CharacterStats, float> missileHitCharacter, Action<Transform, CharacterStats> missileHitAnything, bool isHoming = false, float homingForce = 0, bool isGrowing = false, float growingForce = 1, float maxScaleValue = 1, float lifeSteal = 1)
	{
		rb = GetComponent<Rigidbody>();
		this.parent = parent;
		this.damageToApply = damageToApply;
		HitCharacter = missileHitCharacter;
		HitAnything = missileHitAnything;
		this.isHoming = isHoming;
		this.homingForce = homingForce;
		this.bouncesLeft = bounces;
		this.isGrowing = isGrowing;
		growVector = new Vector3(growingForce, growingForce, growingForce);
		maxScale = new Vector3(maxScaleValue, maxScaleValue, maxScaleValue);
		this.lifeSteal = lifeSteal;
	}

	public Transform GetParentTransform()
    {
		return parent.transform;
    }

	private void Update()
	{
		lastVelocity = rb.velocity;

		if(isHoming)
        {
			Vector3 newVelocity = lastVelocity;
			//detect any character in area
			if (enemyCheckCurrentTime > 0)
				enemyCheckCurrentTime -= Time.deltaTime;
			else
            {
				enemyCheckCurrentTime = enemyCheckFrequency;
				Collider[] cols = Physics.OverlapSphere(this.transform.position, homingRange);
				
				foreach(Collider col in cols)
                {
					if(col.TryGetComponent(out CharacterStats stats))
                    {
						//don't home on dead people
						if (stats.isDead)
							continue;
						//don't home on yourself
						if (stats == parent)
							continue;

						float mag = lastVelocity.magnitude;
						Vector3 dir = Vector3.Slerp(lastVelocity.normalized, (stats.transform.position - this.transform.position).normalized, homingForce);

						newVelocity = dir * mag;
                    }
                }
            }
			//curve a little in that direction
			rb.velocity = newVelocity;
        }

		if(isGrowing)
        {
			if(this.transform.localScale.magnitude < maxScale.magnitude)
				this.transform.localScale = this.transform.localScale + growVector;
		}
	}

	private void CreateHitImpact()
	{
		if (hitImpactParticles != null)
		{
			GameObject createdImpact = Instantiate(hitImpactParticles, this.transform.position, Quaternion.identity);
			createdImpact.transform.localScale = this.transform.localScale;
			Destroy(createdImpact, hitImpactParticlesLifetime);
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.transform.root.CompareTag("Bullet"))
		{
			return;
		}

		HitAnything?.Invoke(this.transform, parent);

		if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
		{
			if (targetCharacterStats == parent && !bounced)
				return;

			if (targetCharacterStats.isDead)
				return;

			if (!other.transform.CompareTag("Player") && !parent.CompareTag("Player"))
				damageToApply *= 0.5f;

			targetCharacterStats.DealDamge(damageToApply);
			HitCharacter?.Invoke(parent, targetCharacterStats, damageToApply);
			if (lifeSteal > 0)
			{
				parent.Heal(damageToApply * lifeSteal);
				parent.PlayHealSound();
				GameObject spawnedParticles = null;

				spawnedParticles = MonoBehaviour.Instantiate(Resources.Load<GameObject>("HealingParticles"), parent.transform);

				MonoBehaviour.Destroy(spawnedParticles, 1.5f);
			}

			if (other.transform.root.TryGetComponent(out TargetingAI targeting))
			{
				targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), damageToApply);
			}

			CreateHitImpact();
			Destroy(this.gameObject);
		}



		//if (bouncesLeft > 0 && other.transform.CompareTag("Obstacles"))
  //      {
		//	RaycastHit hit;
		//	if (Physics.Raycast(transform.position, transform.forward, out hit))
		//	{
		//		Debug.Log("Point of contact: " + hit.point);
		//	}

		//	bounced = true;
  //          var direction = Vector3.Reflect(lastVelocity.normalized, hit.point.normalized);
  //          GetComponent<Rigidbody>().velocity = direction * Mathf.Max(lastVelocity.magnitude, 1f);
  //          bouncesLeft--;
  //      }
  //      else if(other.transform.CompareTag("Obstacles"))
		//{
  //          CreateHitImpact();
  //          Destroy(this.gameObject);
  //      }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (!collision.transform.CompareTag("Obstacles"))
			return;

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
}
