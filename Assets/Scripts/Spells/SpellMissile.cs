using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class SpellMissile : MonoBehaviour
{
	private bool _initialized = false;
	private SpellMissileVisuals _visuals;
	private Spell _spell;
	private CharacterStats parent = null;

	private Timer enemyCheckTimer;
	[SerializeField] private float enemyCheckFrequency;
	private bool isEnemyCheckReady = false;

	private int bouncesLeft = 0;
	private bool bounced = false;

	public event Action<CharacterStats, CharacterStats, float> HitCharacter = null;
	public event Action<Transform, CharacterStats> HitAnything = null;
	private Rigidbody rb;
	private Vector3 lastVelocity;

    private void Awake()
    {
		_visuals = GetComponent<SpellMissileVisuals>();
		rb = GetComponent<Rigidbody>();
	}

    private void Start()
    {
		enemyCheckTimer = new Timer(enemyCheckFrequency, () => isEnemyCheckReady = true);
	}

    public void Initialize(CharacterStats owner, Spell spell, Vector3 direction)
	{
		_visuals.Initialize(spell.spellType);

		parent = owner;
		_spell = spell;

		Vector3 recoil = GetSpellRecoil(spell);
		rb.velocity = (direction + recoil) * spell.currentData.spellSpeed;

		_initialized = true;
	}

	public void SubscribeToEvents(Spell spell)
    {
		HitCharacter = spell.currentData.MissileHitCharacter;
		HitAnything = spell.currentData.MissileHitAnything;
	}

	private Vector3 GetSpellRecoil(Spell spell)
    {
		return new Vector3(UnityEngine.Random.Range(-spell.currentData.recoil, spell.currentData.recoil), 0.0f, 
			UnityEngine.Random.Range(-spell.currentData.recoil, spell.currentData.recoil));
	}

	public Transform GetParentTransform()
    {
		return parent.transform;
    }

	private void Update()
	{
		if (!_initialized)
			return;

		float time = Time.deltaTime;
		enemyCheckTimer.Update(time);

		lastVelocity = rb.velocity;

		if(_spell.currentData.isHoming)
        {
			Vector3 newVelocity = lastVelocity;
			//detect any character in area
			if (isEnemyCheckReady)
            {
				Collider[] cols = Physics.OverlapSphere(this.transform.position, _spell.currentData.homingRange);
				
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
						Vector3 dir = Vector3.Slerp(lastVelocity.normalized, (stats.transform.position - this.transform.position).normalized, _spell.currentData.homingForce);

						newVelocity = dir * mag;
                    }
                }
            }
			//curve a little in that direction
			rb.velocity = newVelocity;
        }

		if(_spell.currentData.isGrowing)
        {
			if(this.transform.localScale.magnitude < _spell.currentData.maxScale)
				this.transform.localScale = this.transform.localScale * _spell.currentData.growingForce;
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

			//if (!other.transform.CompareTag("Player") && !parent.CompareTag("Player"))
			//	_spell.currentData.damage *= 0.5f;

			targetCharacterStats.DealDamge(_spell.currentData.damage);
			HitCharacter?.Invoke(parent, targetCharacterStats, _spell.currentData.damage);
			if (_spell.currentData.lifeSteal > 0 && !targetCharacterStats.isDead)
			{
				parent.Heal(_spell.currentData.damage * _spell.currentData.lifeSteal);
				parent.PlayHealSound();
				GameObject spawnedParticles = null;

				spawnedParticles = MonoBehaviour.Instantiate(Resources.Load<GameObject>("HealingParticles"), parent.transform);

				MonoBehaviour.Destroy(spawnedParticles, 1.5f);
			}

			if (other.transform.root.TryGetComponent(out TargetingAI targeting))
			{
				targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), _spell.currentData.damage);
			}

			_visuals.CreateHitImpact();
			Destroy(this.gameObject);
		}
		if (other.transform.CompareTag("Obstacles"))
		{
			_visuals.CreateHitImpact();
			Destroy(this.gameObject);
		}
    }
}
