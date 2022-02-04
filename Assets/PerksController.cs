using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    public List<SpellBoostScriptable> spellBoostScriptables;
    public Dictionary<int, SpellBoostScriptable> spellBoostDictionary;
    public List<SpellBoost> anySpellBoosts = new List<SpellBoost>();
    public List<SpellBoost> LeftSpellBoosts = new List<SpellBoost>();
    public List<SpellBoost> RightSpellBoosts = new List<SpellBoost>();
    public List<SpellBoost> allSpellBoosts = new List<SpellBoost>();

    SlowOnHitEnemyBoost slowOnHit;
    CreateToxicAreaOnHitBoost toxicArea;
    NumberOfBulletsBoost numberOfBulletsBoost;
    FireRateBoost fireRateBoost;
    LowerResourceModifierBoost lowerResourceModifierBoost;
    HealOnHitEnemyBoost healOnHitEnemyBoost;
    HomingBoost homingBoost;
    GrowingBoost growingBoost;
    BouncingBoosts bouncingBoost;
    FastBoost fastBoost;
    MoveSpeedBoost moveBoost;
    HealthBoost healthBoost;

    public static PerksController Instance;
    private GameObject playerRef;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerRef = GameObject.FindGameObjectWithTag("Player");

        slowOnHit = new SlowOnHitEnemyBoost();
        toxicArea = new CreateToxicAreaOnHitBoost();
        numberOfBulletsBoost = new NumberOfBulletsBoost();
        fireRateBoost = new FireRateBoost();
        lowerResourceModifierBoost = new LowerResourceModifierBoost();
        healOnHitEnemyBoost = new HealOnHitEnemyBoost();
        homingBoost = new HomingBoost();
        growingBoost = new GrowingBoost();
        bouncingBoost = new BouncingBoosts();
        fastBoost = new FastBoost();
        moveBoost = new MoveSpeedBoost();
        healthBoost = new HealthBoost();

        LeftSpellBoosts.Add(healOnHitEnemyBoost);
        LeftSpellBoosts.Add(slowOnHit);
        RightSpellBoosts.Add(toxicArea);
        //RightSpellBoosts.Add(bouncingBoost);

        anySpellBoosts.Add(growingBoost);
        anySpellBoosts.Add(numberOfBulletsBoost);
        anySpellBoosts.Add(fireRateBoost);
        anySpellBoosts.Add(lowerResourceModifierBoost);
        anySpellBoosts.Add(homingBoost);
        anySpellBoosts.Add(fastBoost);
        anySpellBoosts.Add(moveBoost);
        anySpellBoosts.Add(healthBoost);

        allSpellBoosts.Add(healOnHitEnemyBoost);
        allSpellBoosts.Add(slowOnHit);
        allSpellBoosts.Add(toxicArea);
       // allSpellBoosts.Add(bouncingBoost);
        allSpellBoosts.Add(growingBoost);
        allSpellBoosts.Add(numberOfBulletsBoost);
        allSpellBoosts.Add(fireRateBoost);
        allSpellBoosts.Add(lowerResourceModifierBoost);
        allSpellBoosts.Add(homingBoost);
        allSpellBoosts.Add(fastBoost);
        allSpellBoosts.Add(moveBoost);
        allSpellBoosts.Add(healthBoost);

        spellBoostDictionary = new Dictionary<int, SpellBoostScriptable>();

        foreach (SpellBoostScriptable spellBoost in spellBoostScriptables)
        {
            spellBoostDictionary.Add(spellBoost.id, spellBoost);
        }
    }

    public void AddHealingOnEnemyHit(Spell spell)
    {
        healOnHitEnemyBoost.ProcessSpellBoost(spell);
    }

    public void AddGrowingBoost(string spellType)
    {
        if(spellType == "harmony")
            growingBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().LeftSpell);
        else
            growingBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().RightSpell);

    }

    public void AddHomingBoost(string spellType)
    {
        if (spellType == "harmony")
            homingBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().LeftSpell);
        else
            homingBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().RightSpell);

    }

    public void AddMissile(string spellType)
    {
        if (spellType == "harmony")
            numberOfBulletsBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().LeftSpell);
        else
            numberOfBulletsBoost.ProcessSpellBoost(playerRef.GetComponent<SpellcastingController>().RightSpell);

    }
}
