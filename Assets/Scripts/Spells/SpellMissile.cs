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

	public event Action<SpellMissileEventData> HitCharacter = null;
	public event Action<SpellMissileEventData> HitAnything = null;
	public event Action<SpellMissile, SpellMissileEventData> MissileUpdated = null;
	private Rigidbody rb;
	public Rigidbody Rigidbody => rb;
	private Vector3 lastVelocity;
	public Vector3 LastFrameVelocity => lastVelocity;

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
		MissileUpdated = spell.currentData.MissileUpdated;
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

		if(isEnemyCheckReady)
        {
			MissileUpdated?.Invoke(this, new SpellMissileEventData(parent, _spell.currentData));
			enemyCheckTimer.RestartTimer();
			isEnemyCheckReady = false;
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

		HitAnything?.Invoke(new SpellMissileEventData(this.transform, parent));

		if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
		{
			if (targetCharacterStats == parent && !bounced)
				return;

			//if (!other.transform.CompareTag("Player") && !parent.CompareTag("Player"))
			//	_spell.currentData.damage *= 0.5f;

			targetCharacterStats.DealDamge(_spell.currentData.damage);
			HitCharacter?.Invoke(new SpellMissileEventData(parent.transform, parent, targetCharacterStats, _spell.currentData));

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
