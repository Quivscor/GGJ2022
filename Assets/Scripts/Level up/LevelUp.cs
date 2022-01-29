using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject choosingPlace;
    [SerializeField]
    private GameObject perksVisualisation;
    private GameObject playerReference;
    public static LevelUp Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    public void ToggleChoosingPhaze(bool toggle)
    {
        choosingPlace?.SetActive(toggle);
    }

    public void ResourceWasChosen(SpellType spellType)
    {
        playerReference.GetComponent<CharacterStats>().AddLevel(spellType);
        ToggleChoosingPhaze(false);
    }

    public void AddRandomLevelForBot(CharacterStats characterStats, WandController wandController)
    {
        SpellType randomSpellType;

        if (Random.Range(0, 2) == 0)
            randomSpellType = SpellType.Harmony;
        else
            randomSpellType = SpellType.Chaos;

            characterStats.AddLevel(randomSpellType);

        SpellBoost randomSpellBost = GetRandomSpellBoost(randomSpellType);

        if(randomSpellType == SpellType.Harmony)
            randomSpellBost.ProcessSpellBoost(wandController.HarmonySpell);
        else
            randomSpellBost.ProcessSpellBoost(wandController.ChaosSpell);
        Debug.Log("Bot leveled up: " + randomSpellBost.description);
    }

    public void PrepareBoostsList(SpellType spellType, int numberOfBoosts)
    {
        List<SpellBoost> preparedBoosts = new List<SpellBoost>();
        int errors = 0;
        for (int i = 0; i < numberOfBoosts; i++)
        {
            SpellBoost randomBoost = GetRandomSpellBoost(spellType);
            if (!preparedBoosts.Contains(randomBoost))
            {
                preparedBoosts.Add(randomBoost);
            }
            else
                i--;

            errors++;
            if (errors >= 10000)
            {
                Debug.LogError("Over 10000 tries in PrepareBoostsList");
                break;
            }
        }
    }
    public SpellBoost GetRandomSpellBoost(SpellType spellType)
    {
        int errors = 0;
        SpellBoost randomBoost;
        while (true) 
        {
            randomBoost = PerksController.Instance.spellBoosts[Random.Range(0, PerksController.Instance.spellBoosts.Count)];
            if (randomBoost.spellType == SpellType.Any || randomBoost.spellType == spellType)
                break;

            errors++;
            if (errors >= 10000)
            {
                Debug.LogError("Over 10000 tries in GetRandomSpellBoost");
                break;
            }

        } 

        return randomBoost;
    }
}