using System;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell")]
public class SpellData : ScriptableObject
{
    //Copy constructor
    public SpellData(SpellData spell)
    {
        numberOfBullets = spell.numberOfBullets;
        recoil = spell.recoil;

        fireRate = spell.fireRate;
        spellSpeed = spell.spellSpeed;
        damage = spell.damage;
        resourceCost = spell.resourceCost;
        costModifier = spell.costModifier;
        numberOfBounces = spell.numberOfBounces;
        range = spell.range;
        bulletSize = spell.bulletSize;
        lifeSteal = spell.lifeSteal;

        isHoming = spell.isHoming;

        isGrowing = spell.isGrowing;
        growingForce = spell.growingForce;
        maxScale = spell.maxScale;
        chanceToCreateForbiddenArea = spell.chanceToCreateForbiddenArea;

        homingForce = spell.homingForce;
        chanceToHomingMissile = spell.chanceToHomingMissile;
        chanceToHomingMissileStep = spell.chanceToHomingMissileStep;
        angleBetweenShots = spell.angleBetweenShots;
    }

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
    public float angleBetweenShots = 8f;

    public Action<CharacterStats, CharacterStats, float> MissileHitCharacter = null;
    public Action<Transform, CharacterStats> MissileHitAnything = null;
}