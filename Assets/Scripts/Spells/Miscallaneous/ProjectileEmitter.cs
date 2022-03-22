using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class ProjectileEmitter : MonoBehaviour
{
	private Timer _fireTimer;
	private SpellMissile _missile;
	private bool _canFire = false;
	private float _projectileCount;

    private void Awake()
    {
		//Assume this gameobject is always attached to a spellmissile
		_missile = GetComponent<SpellMissile>();
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeBetweenFiring">IF SET TO ZERO, RIP MEMORY</param>
	/// <param name="subProjectileCount">If set to zero, will use SpellData.NumberOfBullets</param>
    public void Initialize(float timeBetweenFiring = .5f, int subProjectileCount = 0)
    {
		//if set to zero, say goodbye to your memory
		_fireTimer = new Timer(timeBetweenFiring, () => _canFire = true);

		if (subProjectileCount <= 0)
			_projectileCount = _missile.Spell.Data.GetStat(SpellStatType.BulletCount).Max;
		else
			_projectileCount = subProjectileCount;
	}

    // Update is called once per frame
    void Update()
    {
		_fireTimer.Update(Time.deltaTime);

		if (_canFire)
		{
			Fire(_missile, _missile.Right);
			Fire(_missile, -_missile.Right);
		}
	}

	private void Fire(SpellMissile missile, Vector3 direction)
	{
		for (int i = 0; i < _projectileCount; i++)
		{
			var firedProjectile = GameObject.Instantiate(missile, missile.transform.position, Quaternion.identity);
			firedProjectile.Initialize(missile.Parent, missile.Spell, CalculateBulletDirection(missile.Spell.Data, i, direction));
			firedProjectile.ResetEvents();
			firedProjectile.SetIgnoreTagList(missile.BulletIgnoreTagList);

			//destroy emitter on duped projectile to prevent killing Unity
			ProjectileEmitter emitter = firedProjectile.GetComponent<ProjectileEmitter>();
			Destroy(emitter);
		}
		_fireTimer.RestartTimer();
		_canFire = false;
	}

	private Vector3 CalculateBulletDirection(SpellData data, int index, Vector3 direction)
	{
		if (index == 0)
			return direction;

		float shotangle = data.GetStat(SpellStatType.ShotAngle).Max;
		Vector3 right = -Vector3.Cross(direction, Vector3.up);
		float angle = shotangle + ((index - 1) / 2 * shotangle);
		if (index % 2 != 0)
			return Vector3.RotateTowards(direction, right, Mathf.Deg2Rad * angle, 0f).normalized;
		else
			return Vector3.RotateTowards(direction, -right, Mathf.Deg2Rad * angle, 0f).normalized;
	}
}
