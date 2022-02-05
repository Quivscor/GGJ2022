using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksController : MonoBehaviour
{
    [SerializeField]
    private List<SpellBoostScriptable> allSpellBoosts;
    public List<SpellBoostScriptable> AllSpellBoosts => allSpellBoosts;

    public static PerksController Instance;
    private GameObject playerRef;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerRef = GameObject.FindGameObjectWithTag("Player");
    }
}
