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

    public event Action<SpellType> SpellTypeChanged = null;
    private SpellType lastSpellType = SpellType.Harmony;


    private void Awake()
	{
        harmonySpell.Initialize(missilesSpawnPoint);
        chaosSpell.Initialize(missilesSpawnPoint);
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
        if(spellType == SpellType.Harmony)
        {   
            harmonySpell.CastSpell();
        }
        else if (spellType == SpellType.Chaos)
        {
            chaosSpell.CastSpell();
        }


        if (lastSpellType != spellType)
		{
            lastSpellType = spellType;
            SpellTypeChanged?.Invoke(lastSpellType);
        }
    }
}
