using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class SpellMissile : MonoBehaviour
{
	private bool _initialized = false;
	[SerializeField] private string[] bulletIgnoreTagList;
	public string[] BulletIgnoreTagList => bulletIgnoreTagList;
	[SerializeField] private string[] bulletBounceTagList;
	public string[] BulletBounceTagList => bulletBounceTagList;
	private SpellMissileVisuals _visuals;
	private Spell _spell;
	public Spell Spell => _spell;
	private CharacterStats parent = null;
	public CharacterStats Parent => parent;

	private Timer enemyCheckTimer;
	[SerializeField] private float enemyCheckFrequency;
	private bool isEnemyCheckReady = true;

	private Timer lifetimeTimer;

	private float _lifetime = 0;
	public float LifeTime => _lifetime;
	private Vector3 _forward;
	public Vector3 Forward => _forward;
	private Vector3 _right;
	public Vector3 Right => _right;

	private int _bouncesLeft = 0;
	private bool _bounced = false;

	public event Action<SpellMissileEventData> HitCharacter = null;
	public event Action<SpellMissileEventData> HitAnything = null;
	public event Action<SpellMissile, SpellMissileEventData> MissileUpdated = null;
	public event Action<SpellMissile, SpellMissileEventData> MissileInitialized = null;
	private Rigidbody rb;
	public Rigidbody Rigidbody => rb;
	private Vector3 lastVelocity;
	public Vector3 LastFrameVelocity => lastVelocity;
	private List<Collider> _ignoredColliders;
	private Collider _collider;

    private void Awake()
    {
		_visuals = GetComponent<SpellMissileVisuals>();
		rb = GetComponent<Rigidbody>();

		//if bullet has more than non trigger collider this will break
		Collider[] colliders = GetComponentsInChildren<Collider>();
		foreach(Collider col in colliders)
        {
			if (!col.isTrigger)
				_collider = col;
        }
	}

    private void Start()
    {
		_ignoredColliders = new List<Collider>();
	}

    public void Initialize(CharacterStats owner, Spell spell, Vector3 direction)
	{
		_visuals.Initialize(spell.spellType);

		parent = owner;
		_spell = spell;
		_bouncesLeft = spell.currentData.numberOfBounces;

		if (spell.currentData.bulletLifetime != Mathf.Infinity)
			lifetimeTimer = new Timer(spell.currentData.bulletLifetime, Dispose);

		Vector3 recoil = GetSpellRecoil(spell);
		rb.velocity = (direction + recoil).normalized;
		//make sure to return with normalized vector from this event
		MissileInitialized?.Invoke(this, new SpellMissileEventData(parent, _spell.currentData));
		rb.velocity *= spell.currentData.spellSpeed;
		RecalculateMissileVectors();

		_initialized = true;
	}

	public void SubscribeToEvents(Spell spell)
    {
		HitCharacter = spell.currentData.MissileHitCharacter;
		HitAnything = spell.currentData.MissileHitAnything;
		MissileUpdated = spell.currentData.MissileUpdated;
		MissileInitialized = spell.currentData.MissileInitialized;
	}

	public void ResetEvents()
	{
		HitCharacter = null;
		HitAnything = null;
		MissileUpdated = null;
		MissileInitialized = null;
	}

	public void SetIgnoreTagList(string[] ignoreTags)
    {
		SetTagList(ref bulletIgnoreTagList, ignoreTags);
    }

	public void SetBounceTagList(string[] ignoreTags)
    {
		SetTagList(ref bulletBounceTagList, ignoreTags);
    }

	private void SetTagList(ref string[] listToSet, string[] ignoreTags)
    {
		string[] list = new string[ignoreTags.Length + listToSet.Length];
		listToSet.CopyTo(list, 0);
		ignoreTags.CopyTo(list, listToSet.Length);
		listToSet = list;
	}

	private Vector3 GetSpellRecoil(Spell spell)
    {
		return new Vector3(UnityEngine.Random.Range(-spell.currentData.recoil, spell.currentData.recoil), 0.0f, 
			UnityEngine.Random.Range(-spell.currentData.recoil, spell.currentData.recoil));
	}

	public void RecalculateMissileVectors()
    {
		_forward = rb.velocity.normalized;
		_right = -Vector3.Cross(_forward, Vector3.up);
	}

	public Transform GetParentTransform()
    {
		//prevents from ruining orbit spell boost together with DashTurret
		if (parent == null)
			return this.transform;

		return parent.transform;
    }

	private void Update()
	{
		if (!_initialized)
			return;

		float time = Time.deltaTime;
		lifetimeTimer?.Update(time); //if spell.bulletLifetime = Infinity -> ignore that

		lastVelocity = rb.velocity;

        MissileUpdated?.Invoke(this, new SpellMissileEventData(parent, _spell.currentData));
        isEnemyCheckReady = false;

        //update time at the end because of event
        _lifetime += time;
	}

    private void OnTriggerEnter(Collider other)
    {
		if (CheckBulletTags(bulletIgnoreTagList, other.transform))
		{
			return;
		}

		HitAnything?.Invoke(new SpellMissileEventData(this.transform, parent));

		if (CheckBulletTags(bulletBounceTagList, other.transform))
			return;
		else if (other.transform.root.TryGetComponent(out CharacterStats targetCharacterStats))
		{
			Debug.Log(_bounced);
			if (targetCharacterStats == parent && !_bounced)
			{
				Debug.Log(_bounced);
				return;

			}

			// This should be handled by whatever is receiving damage, through a buff or resistance of sorts
			//if (!other.transform.CompareTag("Player") && !parent.CompareTag("Player"))
			//	_spell.currentData.damage *= 0.5f;

			targetCharacterStats.DealDamge(_spell.currentData.damage);
			if (parent != null)
				HitCharacter?.Invoke(new SpellMissileEventData(parent.transform, parent, targetCharacterStats, _spell.currentData));
			else
				HitCharacter?.Invoke(new SpellMissileEventData((CharacterStats)null, targetCharacterStats, _spell.currentData));

			//move to damage handling
			//if (other.transform.root.TryGetComponent(out TargetingAI targeting))
			//{
			//	targeting.UpdateThreat(targeting.GetTargetFromTransform(parent.transform), _spell.currentData.damage);
			//}

			Dispose();
		}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CheckBulletTags(bulletBounceTagList, collision.collider.transform))
        {
			if (!_ignoredColliders.Contains(collision.collider))
            {
				_ignoredColliders.Add(collision.collider);
				Physics.IgnoreCollision(_collider, collision.collider, true);
			}
			return;
		}

        if (_bouncesLeft > 0)
        {
            _bounced = true;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            GetComponent<Rigidbody>().velocity = direction * Mathf.Max(lastVelocity.magnitude, 1f);
            _bouncesLeft--;
        }
        else
        {
            Dispose();
        }

    }
    private bool CheckBulletTags(string[] tagList, Transform t)
    {
		foreach(string s in tagList)
        {
			if (t.CompareTag(s))
				return true;
        }
		return false;
    }

	private void Dispose()
    {
		_visuals.CreateHitImpact();
		Destroy(this.gameObject);
	}
}
