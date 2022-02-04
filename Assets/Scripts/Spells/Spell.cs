using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Spell
{
    public SpellType spellType;
    public int numberOfBullets = 1;
    public float recoil = 0.05f;

    public float fireRate = 1f; // How many attacks per 1 
    public float spellSpeed = 10f; // Abstract number, to adjust
    public float damage = 10f; // Abstract
    public float resourceCost = 10f;
    public float costModifier = 1f;
    public int numberOfBounces = 0;
    public float range = 10f;
    public float bulletSize = 1f;
    public float lifeSteal = 0f;

    public bool isHoming = false;

    public bool isGrowing = false;
    public float growingForce = 0;
    public float maxScale = 1;
    public float chanceToCreateForbiddenArea = 0f;

    public float homingForce = 0.35f;
    public float chanceToHomingMissile = 0f;
    public float chanceToHomingMissileStep = 0f;

    //public AudioSource audioSourceSpell;

    private float currentFireRate = 0.0f;
    private float currentRecoil = 0.0f;
    private float finalResourceCost = 0f;

    private bool isOnCooldown = false;
    public float angleBetweenShots = 8f;

    private List<SpellBoost> spellBoosts;
    public List<SpellBoost> SpellBoosts => spellBoosts;

    public event Action<CharacterStats, CharacterStats, float> MissileHitCharacter = null;
    public event Action<Transform, CharacterStats> MissileHitAnything = null;

    public void AddSpellBoost(SpellBoost boost)
    {
        SpellBoosts.Add(boost);
        RecalculateSpellBoosts();
    }

    public void RemoveSpellBoost(SpellBoost boost)
    {
        SpellBoosts.Remove(boost);
        RecalculateSpellBoosts();
    }

    public void RecalculateSpellBoosts()
    {
        //spellBoosts.Sort(); by tags

        foreach(SpellBoost boost in spellBoosts)
        {
            boost.ProcessSpellBoost(this);
        }
    }
}
