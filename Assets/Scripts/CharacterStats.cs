using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ResourceChanged : UnityEvent <float> { }

public class CharacterStats : MonoBehaviour
{
    [Header("Debug options")]
    [SerializeField]
    private bool infiniteResources = false;

    [Header("Stats")]
    [SerializeField]
    private Image healthBarFill;

    [Header("Stats")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float maxResource = 100;

    [Header("Event actions")]
    [SerializeField]
    private ResourceChanged resourceChanged;
    private ResourceChanged healthChanged;
    public Action<CharacterStats> died;

    private float primaryResource;
    public float Health { get => health;}
    public float MaxHealth { get => maxHealth; }

    void Start()
    {
        Heal(MaxHealth);
        primaryResource = maxResource / 2f;
        resourceChanged?.Invoke(primaryResource / maxResource);
    }

    public void Heal(float value)
    {
        health += value;
        if (health > MaxHealth)
            health = MaxHealth;

        //healthChanged?.Invoke(health/maxHealth);
        if(healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;
    }

    public void DealDamge(float value)
    {
        health -= value;
        if (health <= 0)
        {
            Debug.LogError(this.name + " DIED");
            died?.Invoke(this);
        }
            
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;
        //healthChanged?.Invoke(health / maxHealth);
    }

    public void CastTestSpell(int spellType)
    {
        SpendResource(10, (SpellType)spellType);
    }
    public bool HaveEnoughResource(float value, SpellType spellType)
    {
        if (spellType == SpellType.Harmony)
        {
            if (primaryResource >= value)
                return true;
            else
                return false;
        }
        else if (maxResource - primaryResource >= value)
        {
             return true;
        }
        else
            return false;
    }

    public void SpendResource(float value, SpellType spellType)
    {
        if(!infiniteResources)
        {
            if (spellType == SpellType.Harmony)
            {
                primaryResource -= value;
            }
            else
            {
                primaryResource += value;
            }
        }

        resourceChanged?.Invoke(primaryResource / maxResource);
    }
}
