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

    private void Update()
    {
        // TEST
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ProcessSpell(true, false);
        }
    }
    public void ProcessSpell(bool primarySpell, bool secondarySpell)
	{
        if (primarySpell)
            ProcessSpellCast(SpellType.Harmony);
        else if (secondarySpell)
            ProcessSpellCast(SpellType.Chaos);
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
