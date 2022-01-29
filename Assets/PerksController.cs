using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    SlowOnHitEnemyBoost slowOnHit = new SlowOnHitEnemyBoost();
    CreateToxicAreaOnHitBoost toxicArea = new CreateToxicAreaOnHitBoost();
    NumberOfBulletsBoost numberOfBulletsBoost = new NumberOfBulletsBoost();
    FireRateBoost fireRateBoost = new FireRateBoost();
    LowerResourceModifierBoost lowerResourceModifierBoost = new LowerResourceModifierBoost();

    public static PerksController instance;
    public static PerksController Instance
    {
        get
        {
            if (instance == null)
                instance = new PerksController();

            return instance;
        }
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
