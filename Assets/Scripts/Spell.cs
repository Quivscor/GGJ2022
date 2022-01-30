using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private float recoil = 0.05f;

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
    private float costModifier = 1f;
    [SerializeField]
    private int numberOfBounces = 0;
    [SerializeField]
    private float range = 10f;
    [SerializeField]
    private float bulletSize = 1f;

    private bool isHoming = false;
    private float homingForce = 0.35f;
    private CharacterStats characterStats;
    private float chanceToHomingMissile = 0f;
    private float chanceToHomingMissileStep = 0f;

    public int NumberOfBullets { get => numberOfBullets; set => numberOfBullets = value; }
    public float Recoil { get => recoil; set => recoil = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float SpellSpeed { get => spellSpeed; set => spellSpeed = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ResourceCost { get => resourceCost; set => resourceCost = value; }
    public int NumberOfBounces { get => numberOfBounces; set => numberOfBounces = value; }
    public float Range { get => range; set => range = value; }
    public float BulletSize { get => bulletSize; set => bulletSize = value; }
    public bool IsHoming { get => isHoming; set => isHoming = value; }
    public float HomingForce { get => homingForce; set => homingForce = value; }
    public float ChanceToHomingMissileStep { get => chanceToHomingMissileStep; set => chanceToHomingMissileStep = value; }

    private float currentFireRate = 0.0f;
    private float currentRecoil = 0.0f;

    private bool isOnCooldown = false;
    private float angleBetweenShots = 8f;

    public event Action<CharacterStats, CharacterStats, float> MissileHitCharacter = null;
    public event Action<Transform, CharacterStats> MissileHitAnything = null;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();

        RecalculateFireRate();
        currentRecoil = recoil;

        //debug AF
        HomingBoost boost = new HomingBoost();
        boost.ProcessSpellBoost(this);
    }

    public void RecalculateFireRate()
    {
        currentFireRate = 1.0f / fireRate;
    }

	private void Start()
	{

        if (spellType == SpellType.Harmony)
        {
            PerksController.Instance.AddHealingOnEnemyHit(this);
        }
    }

	public Vector3 CalculatedRecoil() => new Vector3(UnityEngine.Random.Range(-currentRecoil, currentRecoil), 0.0f, UnityEngine.Random.Range(-currentRecoil, currentRecoil));
    public Vector3 CalculatedAngleBetweenShots(float angle) => new Vector3(angle, 0f, angle);
    public void CastSpell()
    {
        if (isOnCooldown)
            return;

        if (characterStats.HaveEnoughResource(ResourceCost * costModifier, spellType))
        {
            characterStats.SpendResource(ResourceCost * costModifier, spellType);



            for (int i = 0; i < numberOfBullets; i++)
            {
                Vector3 direction = missilesSpawnPoint.transform.forward + CalculatedRecoil();

                if (i != 0)
                {
                    direction = CalculateBulletAngle(i);
                }
               
                if(Random.Range(0, 100) < chanceToHomingMissile)
                {
                    IsHoming = true;
                    chanceToHomingMissile = 0f;
                }
                else
                {
                    chanceToHomingMissile += chanceToHomingMissileStep;
                }

                direction.Normalize();

                var bulletShot = Instantiate(bullet, missilesSpawnPoint.transform.position, Quaternion.identity);
                bulletShot.Initialize(characterStats, damage, numberOfBounces, MissileHitCharacter, MissileHitAnything, IsHoming, homingForce);
                bulletShot.GetComponent<Rigidbody>().velocity = direction * spellSpeed;
                IsHoming = false;

            }
            StartCoroutine(EnableCooldown());
        }
    }

    private Vector3 CalculateBulletAngle(int index)
    {
        float angle = angleBetweenShots + ((index - 1)/2 * angleBetweenShots);
        if (index % 2 != 0)
        {
            return Vector3.RotateTowards(missilesSpawnPoint.transform.forward, missilesSpawnPoint.transform.right, Mathf.Deg2Rad * angle, 0f);
        }
        else
        {
            return Vector3.RotateTowards(missilesSpawnPoint.transform.forward, -missilesSpawnPoint.transform.right, Mathf.Deg2Rad * angle, 0f);
        }
    }

    private IEnumerator EnableCooldown()
	{
        isOnCooldown = true;
        yield return new WaitForSeconds(currentFireRate);
        isOnCooldown = false;
    }

    public void ChangeCostModifier(float value)
    {
        costModifier += value;

        if (costModifier < 0.25)
        {
            costModifier = 0.25f;
        }
    }
}
