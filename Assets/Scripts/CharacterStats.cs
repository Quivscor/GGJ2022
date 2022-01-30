using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ResourceChanged : UnityEvent <float> { }

public class CharacterStats : MonoBehaviour
{
    [Header("Debug options")]
    [SerializeField]
    private bool infiniteResources = false;
    [SerializeField]
    private bool immortality = false;

    [Header("References")]
    [SerializeField]
    private Image healthBarFill;

    [Header("Stats")]
    [SerializeField]
    private float health;
    [SerializeField]
    public float maxHealth = 100;
    [SerializeField]
    private float maxResource = 100;
    [SerializeField]
    private int harmonyLevel = 0;
    [SerializeField]
    private int chaosLevel = 0;

    [Header("Event actions")]
    [SerializeField]
    private ResourceChanged resourceChanged;
    [SerializeField]
    private ResourceChanged healthChanged;
    public Action<CharacterStats> died;
    public bool isDead { get; private set; }

    private float primaryResource;
    public float Health { get => health;}
    public float MaxHealth { get => maxHealth; }
    public int HarmonyLevel { get => harmonyLevel; }
    public int ChaosLevel { get => chaosLevel; }

    public AudioSource healAudio;

    public void PlayHealSound()
    {
        if (healAudio != null && transform.CompareTag("Player"))
            healAudio.Play();
    }

    void Start()
    {
        health = MaxHealth;
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;
        primaryResource = maxResource / 2f;
        resourceChanged?.Invoke(primaryResource / maxResource);
        isDead = false;
    }


    public void Heal(float value)
    {
        health += value;
        if (health > MaxHealth)
            health = MaxHealth;

        healthChanged?.Invoke(health/maxHealth);
        if(healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;
    }

    public void Resurect()
    {
        health = maxHealth;
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;
        isDead = false;
        GetComponent<CharacterMaterialReplacer>().RevertMat();
    }

    public void DealDamge(float value)
    {
        if(!immortality)
        {
            health -= value;
            if (health <= 0 && !isDead)
            {
                Debug.LogError(this.name + " DIED");
                died?.Invoke(this);
                GetComponent<CharacterMaterialReplacer>().UseNewMat();
                isDead = true;
            }

            if (healthBarFill != null)
                healthBarFill.fillAmount = health / maxHealth;
            healthChanged?.Invoke(health / maxHealth);
        }

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

    public void AddLevel(SpellType spellType)
    {
        if (spellType == SpellType.Harmony)
            harmonyLevel++;
        else
            chaosLevel++;

        if(transform.root.CompareTag("Player"))
            maxHealth += maxHealth * 0.15f;
        else
            maxHealth += maxHealth * 0.1f;
    }
}
