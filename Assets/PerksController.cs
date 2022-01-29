using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    public List<SpellBoost> spellBoosts = new List<SpellBoost>();
    SlowOnHitEnemyBoost slowOnHit;
    CreateToxicAreaOnHitBoost toxicArea;
    NumberOfBulletsBoost numberOfBulletsBoost;
    FireRateBoost fireRateBoost;
    LowerResourceModifierBoost lowerResourceModifierBoost;

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

        spellBoosts.Add(slowOnHit);
        spellBoosts.Add(toxicArea);
        spellBoosts.Add(numberOfBulletsBoost);
        spellBoosts.Add(fireRateBoost);
        spellBoosts.Add(lowerResourceModifierBoost);
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
}
