using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
    private Animator[] planesAnimator;
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
    public List<SpellBoost> preparedBoostsBots = new List<SpellBoost>();

    public MonumentsRise monumentsRise;
    public AudioSource upgradeSource;
    public bool choosingPhaze = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    public void ToggleChoosingPhaze(bool toggle)
    {
        choosingPhaze = toggle;
        choosingPlace?.SetActive(toggle);
    }

    public void ResourceWasChosen(SpellType spellType)
    {
        if(choosingPhaze)
        {
            ToggleChoosingPhaze(false);

            perkBought = false;

            HudController.Instance.TogglePressurePlateInfo(false);
            HudController.Instance.TogglePressEInfo(false);

            chosenResource = spellType;
            playerReference.GetComponent<CharacterStats>().AddLevel(spellType);

            SetMonumentsMaterial(spellType);

            monumentsRise.Invoke(1.5f);
            PrepareBoostsList(spellType, numberOfBoostsToChoose);
            monumentsAnimator.SetTrigger("Show");

            foreach (Animator animator in planesAnimator)
                animator.SetTrigger("HideTrigger");
        }
      
    }

    public void BuyPerkFromMonument(int monumentNumber)
    {
        if (perkBought)
            return;

        perkBought = true;
        if (upgradeSource != null)
            upgradeSource.Play();
        if (chosenResource == SpellType.Left)
            preparedBoosts[monumentNumber].ProcessSpellBoost(playerReference.GetComponent<SpellcastingController>().LeftSpell);
        else
            preparedBoosts[monumentNumber].ProcessSpellBoost(playerReference.GetComponent<SpellcastingController>().RightSpell);

        HudController.Instance.UpdateStatsInfo();

        monumentsRise.Invoke(1.5f);
        monumentsAnimator.SetTrigger("Hide");
        
        MatchController.Instance.ChangeMatchState(MatchState.PRESTART);
        preparedBoosts.Clear();

        HudController.Instance.TurnOffBotsUpgradeInfo();
        HudController.Instance.ToggleRoundEnded(false);
    }

    public void ShowMonumentInfo(int monumentNumber)
    {
        HudController.Instance.ShowPerkInfo(preparedBoosts[monumentNumber].spellName, preparedBoosts[monumentNumber].description, preparedBoosts[monumentNumber].costModifier, chosenResource);
    }

    public void HideMonumentInfo(int monumentNumber)
    {
        HudController.Instance.HidePerkInfo();
    }

    public void TogglePressurePlateInfo(SpellType spellType, bool toggle)
    {
        HudController.Instance.TogglePressurePlateInfo(toggle, spellType);
    }
    private void Update()
    {
        if(Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                ToggleChoosingPhaze(true);
            }
        }


    }

    private void SetMonumentsMaterial(SpellType spellType)
    {
        foreach (GameObject monument in monumentsObjects)
        {
            if (spellType == SpellType.Left)
                monument.GetComponentInChildren<MeshRenderer>().material = harmonyMaterial;
            else
                monument.GetComponentInChildren<MeshRenderer>().material = chaosMaterial;

        }
    }
    public void PrepareBoostsForBots()
    {
        preparedBoostsBots.Clear();
        preparedBoostsBots.AddRange(PerksController.Instance.allSpellBoosts);
    }

    public BotUpgradeInfo AddRandomLevelForBot(CharacterStats characterStats, SpellcastingController SpellcastingController)
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        SpellBoost randomSpellBoost = GetRandomSpellBoostFromList();
        SpellType spellTypeInfo;

        // Jezeli wylosowany pochodzi z harmonii
        if (randomSpellBoost.spellType == SpellType.Left)
        {
            randomSpellBoost.ProcessSpellBoost(SpellcastingController.LeftSpell);
            characterStats.AddLevel(randomSpellBoost.spellType);
            spellTypeInfo = SpellType.Left;

        } // Jezeli wylosowany pochodzi z harmonii
        else if (randomSpellBoost.spellType == SpellType.Right)
        {
            randomSpellBoost.ProcessSpellBoost(SpellcastingController.RightSpell);
            characterStats.AddLevel(randomSpellBoost.spellType);
            spellTypeInfo = SpellType.Right;


        }// Jezeli wylosowany jest Commonem, wylosuj do tego kategorie 
        else
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                randomSpellBoost.ProcessSpellBoost(SpellcastingController.LeftSpell);
                characterStats.AddLevel(SpellType.Left);
                spellTypeInfo = SpellType.Left;

            }
            else
            {
                randomSpellBoost.ProcessSpellBoost(SpellcastingController.RightSpell);
                characterStats.AddLevel(SpellType.Right);
                spellTypeInfo = SpellType.Right;
            }
        }
        if (spellTypeInfo == SpellType.Left)
        {
            characterStats.AddUpgradeIcon(HudController.Instance.harmonyColor);
        }
        else
        {
            characterStats.AddUpgradeIcon(HudController.Instance.chaosColor);

        }


        return new BotUpgradeInfo(characterStats.wizardName, randomSpellBoost.spellName, spellTypeInfo);
    }

    public void PrepareBoostsList(SpellType spellType, int numberOfBoosts)
    {
        List<SpellBoost> availableBoosts = new List<SpellBoost>(PerksController.Instance.anySpellBoosts);

        int randomIndexForSpecialSpell = Random.Range(0, numberOfBoosts);
        for (int i = 0; i < numberOfBoosts; i++)
        { 
            if(i == randomIndexForSpecialSpell)
            {
                if(spellType == SpellType.Left)
                {
                    preparedBoosts.Add(PerksController.Instance.LeftSpellBoosts[Random.Range(0, PerksController.Instance.LeftSpellBoosts.Count)]);
                }
                else
                {
                    preparedBoosts.Add(PerksController.Instance.RightSpellBoosts[Random.Range(0, PerksController.Instance.RightSpellBoosts.Count)]);

                }
             
            }
            else
            {
                var randomBoost = Random.Range(0, availableBoosts.Count);
                preparedBoosts.Add(availableBoosts[randomBoost]);
                availableBoosts.RemoveAt(randomBoost);
            }
            
        }

        foreach (SpellBoost spellboost in preparedBoosts)
        {
            Debug.Log(spellboost.spellName);
        }

    }
    public SpellBoost GetRandomSpellBoostFromList()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(0, preparedBoostsBots.Count);
        SpellBoost spellBoostTemp = preparedBoostsBots[rand];
        preparedBoostsBots.RemoveAt(rand);

        Debug.Log("Random spell: " + spellBoostTemp.spellName);
        return spellBoostTemp;
    }

}
public struct BotUpgradeInfo
{
    public string botname;
    public string spellname;
    public SpellType spellType;

    public BotUpgradeInfo(string wizardName, string spellName, SpellType spellType) : this()
    {
        this.botname = wizardName;
        this.spellname = spellName;
        this.spellType = spellType;
    }
}