using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public class MonumentsRise : UnityEvent<float> { }
public class UpgradesController : MonoBehaviour
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
    public static UpgradesController Instance;
    private SpellType chosenResource;
    private bool perkBought = false;
    
    public List<SpellBoostScriptable> preparedBoosts = new List<SpellBoostScriptable>();

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
            HudController.Instance.TogglePressurePlateInfo(false);
            HudController.Instance.TogglePressEInfo(false);
            SetMonumentsMaterial(spellType);
            monumentsRise.Invoke(1.5f);
            monumentsAnimator.SetTrigger("Show");
            foreach (Animator animator in planesAnimator)
                animator.SetTrigger("HideTrigger");

            ToggleChoosingPhaze(false);
            perkBought = false;
            chosenResource = spellType;
            playerReference.GetComponent<CharacterStats>().AddLevel(spellType);
            PrepareBoostsList(spellType, numberOfBoostsToChoose);
        }
      
    }

    public void GetBoostFromMonument(int monumentNumber)
    {
        if (perkBought)
            return;

        perkBought = true;

        if (upgradeSource != null)
            upgradeSource.Play();

        if (chosenResource == SpellType.Left)
            playerReference.GetComponent<SpellcastingController>().LeftSpell.AddSpellBoost(preparedBoosts[monumentNumber]);
        else
            playerReference.GetComponent<SpellcastingController>().RightSpell.AddSpellBoost(preparedBoosts[monumentNumber]);

        monumentsRise.Invoke(1.5f);
        monumentsAnimator.SetTrigger("Hide");

        HudController.Instance.UpdateStatsInfo();
        MatchController.Instance.ChangeMatchState(MatchState.PRESTART);
        HudController.Instance.TurnOffBotsUpgradeInfo();
        HudController.Instance.ToggleRoundEnded(false);
    }

    public void PrepareBoostsList(SpellType spellType, int numberOfBoosts)
    {
        preparedBoosts.Clear();
        List<SpellBoostScriptable> availableBoosts = new List<SpellBoostScriptable>(PerksController.Instance.AllSpellBoosts);

        for (int i = 0; i < numberOfBoosts; i++)
        {
            var randomBoost = Random.Range(0, availableBoosts.Count);
            preparedBoosts.Add(availableBoosts[randomBoost]);
            availableBoosts.RemoveAt(randomBoost);
        }

    }

    public void ShowMonumentInfo(int monumentNumber)
    {
        HudController.Instance.ShowPerkInfo(preparedBoosts[monumentNumber].spellName, preparedBoosts[monumentNumber].description, preparedBoosts[monumentNumber].resourceModifier, chosenResource);
    }

    public void HideMonumentInfo(int monumentNumber)
    {
        HudController.Instance.HidePerkInfo();
    }

    public void TogglePressurePlateInfo(SpellType spellType, bool toggle)
    {
        HudController.Instance.TogglePressurePlateInfo(toggle, spellType);
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

    #region BoostsForBots
    public void PrepareBoostsForBots()
    {
        preparedBoosts.Clear();
        preparedBoosts.AddRange(PerksController.Instance.AllSpellBoosts);
    }

    public BotUpgradeInfo AddRandomBoostForBot(SpellcastingController spellcastingController, CharacterStats characterStats)
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        SpellBoostScriptable randomSpellBoost = GetRandomBoostForBot();
        SpellType spellTypeInfo;

        Random.InitState(System.DateTime.Now.Millisecond);
        if (Random.Range(0,2) == 0)
        {
            spellcastingController.LeftSpell.AddSpellBoost(randomSpellBoost);
            characterStats.AddLevel(SpellType.Left);
            spellTypeInfo = SpellType.Left;
            characterStats.AddUpgradeIcon(HudController.Instance.harmonyColor);
        }
        else
        {
            spellcastingController.RightSpell.AddSpellBoost(randomSpellBoost);
            characterStats.AddLevel(SpellType.Right);
            spellTypeInfo = SpellType.Right;
            characterStats.AddUpgradeIcon(HudController.Instance.chaosColor);
        }

        return new BotUpgradeInfo(characterStats.wizardName, randomSpellBoost.spellName, spellTypeInfo);
    }

    public SpellBoostScriptable GetRandomBoostForBot()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(0, preparedBoosts.Count);
        SpellBoostScriptable spellBoostTemp = preparedBoosts[rand];
        preparedBoosts.RemoveAt(rand);

        return spellBoostTemp;
    }
    #endregion
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