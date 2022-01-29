using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Spell : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpellMissile bullet;

    [Header("Stats")]
    [SerializeField]
    private SpellType spellType;
    [SerializeField]
    private int numberOfBullets = 1;
    [SerializeField]
    private float accuracy = 100f; // 0 to 100 %?

    private Transform missilesSpawnPoint = null;

    internal void Initialize(Transform missilesSpawnPoint)
	{
        this.missilesSpawnPoint = missilesSpawnPoint;
    }

	[SerializeField]
    private float fireRate = 1f; // How many attacks per 1 second
    [SerializeField]
    private float spellSpeed = 10f; // Abstract number, to adjust
    [SerializeField]
    private float damage = 10f; // Abstract
    [SerializeField]
    private float resourceCost = 10f;
    [SerializeField]
    private float numberOfBounces = 0f;
    [SerializeField]
    private float range = 10f;
    [SerializeField]
    private float bulletSize = 1f;

    private CharacterStats characterStats;

    public int NumberOfBullets { get => numberOfBullets; set => numberOfBullets = value; }
    public float Accuracy { get => accuracy; set => accuracy = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float SpellSpeed { get => spellSpeed; set => spellSpeed = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ResourceCost { get => resourceCost; set => resourceCost = value; }
    public float NumberOfBounces { get => numberOfBounces; set => numberOfBounces = value; }
    public float Range { get => range; set => range = value; }
    public float BulletSize { get => bulletSize; set => bulletSize = value; }

    private float currentFireRate = 0.0f;

    private bool isOnCooldown = false;

	private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();

        currentFireRate = 1.0f / fireRate;
    }

    public void CastSpell()
    {
        if (isOnCooldown)
            return;

        if (characterStats.HaveEnoughResource(ResourceCost, spellType))
        {
            characterStats.SpendResource(ResourceCost, spellType);

            Vector3 direction = missilesSpawnPoint.transform.forward;

            var bulletShot = Instantiate(bullet, missilesSpawnPoint.transform.position, Quaternion.identity);
			bulletShot.Initialize(this.gameObject);
			bulletShot.GetComponent<Rigidbody>().velocity = direction * spellSpeed;

			StartCoroutine(EnableCooldown());
        }
    }

    private IEnumerator EnableCooldown()
	{
        isOnCooldown = true;
        yield return new WaitForSeconds(currentFireRate);
        isOnCooldown = false;

    }
}
