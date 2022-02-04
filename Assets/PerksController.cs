using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    public List<SpellBoostScriptable> spellBoostScriptables;
    public Dictionary<int, SpellBoostScriptable> spellBoostDictionary;

    public static PerksController Instance;
    private GameObject playerRef;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerRef = GameObject.FindGameObjectWithTag("Player");

        spellBoostDictionary = new Dictionary<int, SpellBoostScriptable>();

        foreach (SpellBoostScriptable spellBoost in spellBoostScriptables)
        {
            spellBoostDictionary.Add(spellBoost.id, spellBoost);
        }
    }
}
