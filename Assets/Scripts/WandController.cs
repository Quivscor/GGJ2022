using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WandController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Spell harmonySpell = null;
    [SerializeField]
    private Spell chaosSpell = null;
    [SerializeField] private Animator animator = null;
    [Space(10)]
    [SerializeField] private Transform missilesSpawnPoint = null;
    private CharacterStats characterStats;

    public event Action<SpellType> SpellTypeChanged = null;
    private SpellType lastSpellType = SpellType.Harmony;

    public Spell HarmonySpell { get => harmonySpell; }
    public Spell ChaosSpell { get => chaosSpell; }

    private void Awake()
	{
        harmonySpell.Initialize(missilesSpawnPoint);
        chaosSpell.Initialize(missilesSpawnPoint);
        characterStats = GetComponent<CharacterStats>();
    }

    [SerializeField] float harmonyLastSpellTime = .5f;
    [SerializeField] float chaosLastSpellTime = .5f;
    float harmonyLastSpellTimeCurrent;
    float chaosLastSpellTimeCurrent;

    private void Update()
    {
        if (harmonyLastSpellTimeCurrent > 0)
            harmonyLastSpellTimeCurrent -= Time.deltaTime;

        if (chaosLastSpellTimeCurrent > 0)
            chaosLastSpellTimeCurrent -= Time.deltaTime;
    }

    public float GetSpellCost(bool primarySpell, bool secondarySpell)
    {
        if (primarySpell)
            return harmonySpell.ResourceCost;
        else if (secondarySpell)
            return chaosSpell.ResourceCost;
        else
            return Mathf.NegativeInfinity;
    }

	public void ProcessSpell(bool primarySpell, bool secondarySpell)
	{
        if (primarySpell)
            ProcessSpellCast(SpellType.Harmony);
        else if (secondarySpell)
            ProcessSpellCast(SpellType.Chaos);

        ProcessAnimator(primarySpell, secondarySpell);
    }

    private void ProcessAnimator(bool primarySpell, bool secondarySpell)
	{
        if (primarySpell || secondarySpell)
            animator.SetLayerWeight(1, 1);
        else
            animator.SetLayerWeight(1, 0);

        animator?.SetBool("isAttacking", primarySpell || secondarySpell);
    }


    public void ProcessSpellCast(SpellType spellType)
    {
        if(spellType == SpellType.Harmony && chaosLastSpellTimeCurrent <= 0)
        {
            if (characterStats.HaveEnoughResource(harmonySpell.ResourceCost, SpellType.Harmony))
            {
                harmonyLastSpellTimeCurrent = harmonyLastSpellTime;
            }
            harmonySpell.CastSpell();
        }
        else if (spellType == SpellType.Chaos && harmonyLastSpellTimeCurrent <= 0)
        {
            if (characterStats.HaveEnoughResource(chaosSpell.ResourceCost, SpellType.Chaos))
            {
                chaosLastSpellTimeCurrent = chaosLastSpellTime;
            }
            chaosSpell.CastSpell();
        }

        if (lastSpellType != spellType)
		{
            lastSpellType = spellType;
            SpellTypeChanged?.Invoke(lastSpellType);
        }
    }
}
