using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    private int numberOfBullets = 1;
    [SerializeField]
    private float accuracy = 100f; // 0 to 100 %?
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

    public int NumberOfBullets { get => numberOfBullets; set => numberOfBullets = value; }
    public float Accuracy { get => accuracy; set => accuracy = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float SpellSpeed { get => spellSpeed; set => spellSpeed = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ResourceCost { get => resourceCost; set => resourceCost = value; }
    public float NumberOfBounces { get => numberOfBounces; set => numberOfBounces = value; }
    public float Range { get => range; set => range = value; }
    public float BulletSize { get => bulletSize; set => bulletSize = value; }

    public void CastSpell()
    {
        // Spawn bullet
    }
}
