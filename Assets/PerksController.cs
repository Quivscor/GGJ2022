using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    public List<SpellBoost> anySpellBoosts = new List<SpellBoost>();
    public List<SpellBoost> harmonySpellBoosts = new List<SpellBoost>();
    public List<SpellBoost> chaosSpellBoosts = new List<SpellBoost>();

    SlowOnHitEnemyBoost slowOnHit;
    CreateToxicAreaOnHitBoost toxicArea;
    NumberOfBulletsBoost numberOfBulletsBoost;
    FireRateBoost fireRateBoost;
    LowerResourceModifierBoost lowerResourceModifierBoost;
    HealOnHitEnemyBoost healOnHitEnemyBoost;
    HomingBoost homingBoost;
    GrowingBoost growingBoost;

    public static PerksController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        slowOnHit = new SlowOnHitEnemyBoost();
        toxicArea = new CreateToxicAreaOnHitBoost();
        numberOfBulletsBoost = new NumberOfBulletsBoost();
        fireRateBoost = new FireRateBoost();
        lowerResourceModifierBoost = new LowerResourceModifierBoost();
        healOnHitEnemyBoost = new HealOnHitEnemyBoost();
        homingBoost = new HomingBoost();
        growingBoost = new GrowingBoost();

        harmonySpellBoosts.Add(healOnHitEnemyBoost);
        harmonySpellBoosts.Add(slowOnHit);
        chaosSpellBoosts.Add(toxicArea);
        chaosSpellBoosts.Add(homingBoost);

        anySpellBoosts.Add(growingBoost);
        anySpellBoosts.Add(numberOfBulletsBoost);
        anySpellBoosts.Add(fireRateBoost);
        anySpellBoosts.Add(lowerResourceModifierBoost);
    }
    public void AddBoostToxicArea(Spell spell)
    {
        toxicArea.ProcessSpellBoost(spell);
    }

    public void AddBoostSlowOnHit(Spell spell)
    {
        slowOnHit.ProcessSpellBoost(spell);
    }

    public void AddBoostNumberOfBullets(Spell spell)
    {
        numberOfBulletsBoost.ProcessSpellBoost(spell);
    }

    public void AddBoostFireRate(Spell spell)
    {
        fireRateBoost.ProcessSpellBoost(spell);
    }

    public void AddBoostLowerResourceModifier(Spell spell)
    {
        lowerResourceModifierBoost.ProcessSpellBoost(spell);
    }

    public void AddHealingOnEnemyHit(Spell spell)
    {
        healOnHitEnemyBoost.ProcessSpellBoost(spell);
    }
}
