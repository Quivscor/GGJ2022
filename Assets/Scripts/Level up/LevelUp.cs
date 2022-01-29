using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MonumentsRise : UnityEvent<float> { }
public class LevelUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject choosingPlace;
    [SerializeField]
    private GameObject perksVisualisation;
    [SerializeField]
    private Animator monumentsAnimator;
    [SerializeField]
    private List<GameObject> monumentsObjects = new List<GameObject>();
    [SerializeField]
    private Material harmonyMaterial;
    [SerializeField]
    private Material chaosMaterial;

    [Header("Stats")]
    [SerializeField]
    private int numberOfBoostsToChoose = 3;

    private GameObject playerReference;
    public static LevelUp Instance;
    private SpellType chosenResource;
    private bool perkBought = false;
    
    public List<SpellBoost> preparedBoosts = new List<SpellBoost>();

    public MonumentsRise monumentsRise;

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
        perkBought = false;
        playerReference.GetComponent<CharacterStats>().AddLevel(spellType);
        ToggleChoosingPhaze(false);
        SetMonumentsMaterial(spellType);

        monumentsRise.Invoke(1.5f);
        PrepareBoostsList(spellType, numberOfBoostsToChoose);
        monumentsAnimator.SetTrigger("Show");
    }

    public void BuyPerkFromMonument(int monumentNumber)
    {
        if (perkBought)
            return;

        perkBought = true;
        if(chosenResource == SpellType.Harmony)
            preparedBoosts[monumentNumber].ProcessSpellBoost(playerReference.GetComponent<WandController>().HarmonySpell);
        else
            preparedBoosts[monumentNumber].ProcessSpellBoost(playerReference.GetComponent<WandController>().ChaosSpell);

        monumentsRise.Invoke(1.5f);
        monumentsAnimator.SetTrigger("Hide");
    }

    public void ShowMonumentInfo(int monumentNumber)
    {
        HudController.Instance.ShowPerkInfo(preparedBoosts[monumentNumber].spellName, preparedBoosts[monumentNumber].description);
    }

    public void HideMonumentInfo(int monumentNumber)
    {
        HudController.Instance.HidePerkInfo();
    }

    private void SetMonumentsMaterial(SpellType spellType)
    {
        foreach (GameObject monument in monumentsObjects)
        {
            if (spellType == SpellType.Harmony)
                monument.GetComponentInChildren<MeshRenderer>().material = harmonyMaterial;
            else
                monument.GetComponentInChildren<MeshRenderer>().material = chaosMaterial;

        }
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
        List<SpellBoost> availableBoosts = new List<SpellBoost>(PerksController.Instance.anySpellBoosts);

        if(spellType == SpellType.Harmony)
        {
            availableBoosts.AddRange(PerksController.Instance.harmonySpellBoosts);
        }
        else
        {
            availableBoosts.AddRange(PerksController.Instance.chaosSpellBoosts);
        }

        for (int i = 0; i < numberOfBoosts; i++)
        { 
            var randomBoost = Random.Range(0, availableBoosts.Count);

            preparedBoosts.Add(availableBoosts[randomBoost]);
            availableBoosts.RemoveAt(randomBoost);
        }

        foreach (SpellBoost spellboost in preparedBoosts)
        {
            Debug.Log(spellboost.spellName);
        }
    }
    public SpellBoost GetRandomSpellBoost(SpellType spellType)
    {
        int errors = 0;
        SpellBoost randomBoost;
        while (true) 
        {
            randomBoost = PerksController.Instance.anySpellBoosts[Random.Range(0, PerksController.Instance.anySpellBoosts.Count)];
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
