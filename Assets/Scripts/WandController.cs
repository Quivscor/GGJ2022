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

	private void Awake()
	{
        harmonySpell.Initialize(missilesSpawnPoint);
        chaosSpell.Initialize(missilesSpawnPoint);
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

        if (spellType == SpellType.Chaos)
        {
            chaosSpell.CastSpell();
        }

    }
}
