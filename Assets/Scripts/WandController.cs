using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    [SerializeField]
    private Spell harmonySpell = null;
    [SerializeField]
    private Spell chaosSpell = null;

    public void ProcessHarmonySpell(bool process, SpellType spellType)
    {
        if (!process)
            return;
        
        if(spellType == SpellType.Harmony)
        {
            // Do something
        }

        if (spellType == SpellType.Chaos)
        {

            // Do something
        }

    }
}
