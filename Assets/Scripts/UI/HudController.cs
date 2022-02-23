using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HudController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI roundInfo;
    [SerializeField]
    private GameObject botsUpgradesHolder;
    [SerializeField]
    private TextMeshProUGUI botsUpgradesTMP;
    [SerializeField]
    private GameObject roundEndedObject;

    [SerializeField]
    [Header("Perk Info")]
    private TextMeshProUGUI perkNameTMP;
    [SerializeField]
    private TextMeshProUGUI perkDescTMP;
    [SerializeField]
    private TextMeshProUGUI resourceUseTMP;
    [SerializeField]
    private Animator perkAnimator;
    [SerializeField]
    private Image perkFrame;

    [Header("Stats info")]
    [SerializeField] Animator statsInfoAnimator;
    [SerializeField] private GameObject statsHolder;
    [SerializeField] private TextMeshProUGUI neutralStats;
    [SerializeField] private TextMeshProUGUI manaCostHarmony;
    [SerializeField] private TextMeshProUGUI manaCostChaos;


    [SerializeField]
    private TextMeshProUGUI damageHarmony;
    [SerializeField]
    private TextMeshProUGUI damageChaos;

    [SerializeField]
    private TextMeshProUGUI numberOfBulletsHarmony;
    [SerializeField]
    private TextMeshProUGUI numberOfBulletsChaos;

    [SerializeField]
    private TextMeshProUGUI fireRateHarmony;
    [SerializeField]
    private TextMeshProUGUI fireRateChaos;

    [SerializeField]
    private TextMeshProUGUI bulletSpeedHarmony;
    [SerializeField]
    private TextMeshProUGUI bulletSpeedChaos;

    [SerializeField]
    private TextMeshProUGUI additionallEffectsHarmony;
    [SerializeField]
    private TextMeshProUGUI additionallEffectsChaos;

    public Color harmonyColor;
    public Color chaosColor;

    [Header("Press E Info")]
    [SerializeField]
    private TextMeshProUGUI pressEInfoTMP;
    [SerializeField]
    private GameObject pressEInfo;

    private bool perkInfoVisible = false;
    private GameObject playerRef;
    private Spell harmonySpell;
    private Spell chaosSpell;
    public static HudController Instance;
    private bool statsOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        harmonySpell = playerRef.GetComponent<WandController>().HarmonySpell;
        chaosSpell = playerRef.GetComponent<WandController>().ChaosSpell;

        UpdateStatsInfo();
    }

    public void ToggleRoundEnded(bool toggle)
    {
        roundEndedObject.SetActive(toggle);
    }

    public void ToggleStatsInfo()
    {
        statsOpen = !statsOpen;
        if (statsOpen)
            statsInfoAnimator.SetTrigger("Show");
        else
            statsInfoAnimator.SetTrigger("Hide");

        UpdateStatsInfo();
    }

    public void ToggleStatsInfo(bool toggle)
    {
        statsOpen = toggle;
        if (statsOpen)
            statsInfoAnimator.SetTrigger("Show");
        else
            statsInfoAnimator.SetTrigger("Hide");

        UpdateStatsInfo();
    }

    public void UpdateStatsInfo()
    {

        manaCostHarmony.text = Math.Round(harmonySpell.FinalResourceCost, 2).ToString();
        manaCostChaos.text = Math.Round(chaosSpell.FinalResourceCost, 2).ToString();

        damageHarmony.text = Math.Round(harmonySpell.Damage, 2).ToString();
        damageChaos.text = Math.Round(chaosSpell.Damage, 2).ToString();

        numberOfBulletsHarmony.text = harmonySpell.NumberOfBullets.ToString();
        numberOfBulletsChaos.text = chaosSpell.NumberOfBullets.ToString();  
        
        fireRateHarmony.text = harmonySpell.FireRate.ToString();
        fireRateChaos.text = chaosSpell.FireRate.ToString();

        bulletSpeedHarmony.text = harmonySpell.SpellSpeed.ToString();
        bulletSpeedChaos.text = chaosSpell.SpellSpeed.ToString();

        neutralStats.text = GenerateNeutralStats();

        additionallEffectsHarmony.text = GenerateAdditionalEffectsInfo(harmonySpell);
        additionallEffectsChaos.text = GenerateAdditionalEffectsInfo(chaosSpell);
    }

    private string GenerateNeutralStats()
    {
        string result = "";

        var basicMoveSpeed = playerRef.GetComponent<CharacterMovement>().BasicMovementSpeed;
        var currentMoveSpeed = playerRef.GetComponent<CharacterMovement>().MovementSpeed;

        result += Math.Round(((currentMoveSpeed - basicMoveSpeed) / currentMoveSpeed) * 100f, 2) + "% faster walking\n";

        result += playerRef.GetComponent<CharacterStats>().MaxHealth + " point of max health\n";
        

        return result;
    }

    private string GenerateAdditionalEffectsInfo(Spell spell)
    {
        string result = "";

        if(spell.LifeSteal > 0)
            result += "Attacks heal you by " + spell.LifeSteal * 100f + "% damage dealt\n";
        if (spell.ChanceToHomingMissile > 0)
            result += spell.ChanceToHomingMissile + "% chance for homing missile\n";
        if (spell.ChanceToCreateForbiddenArea> 0)
            result += spell.ChanceToCreateForbiddenArea + "% chance to create forbidden area\n";
        if (spell.MaxScale > 1)
            result += "Attacks grow in air up to " + 1 + spell.MaxScale * 10f + "% in scale\n";
        
        return result;
    }

    public void ShowPerkInfo(string perkName, string perkDesc, float resourceUse, SpellType spellType)
    {
        resourceUseTMP.text = "";

        if (resourceUse > 0)
        {
            resourceUseTMP.color = Color.red;
            resourceUseTMP.text += "+";
        }
        else
            resourceUseTMP.color = Color.green;

        resourceUseTMP.text += resourceUse * 100f + "% ";

        if (spellType == SpellType.Harmony)
        {
            perkNameTMP.color = harmonyColor;
            perkFrame.color = harmonyColor;
            resourceUseTMP.text += "Harmony use";
        }
        else
        {
            perkNameTMP.color = chaosColor;
            perkFrame.color = chaosColor;
            resourceUseTMP.text += "Chaos use";
        }

        perkNameTMP.text = perkName;
        perkDescTMP.text = perkDesc;

        perkAnimator.SetTrigger("Show");

    }

    public void TogglePressEInfo(bool toggle, string text = "Press E")
    {
        pressEInfoTMP.text = text;
        pressEInfo.SetActive(toggle);
    }
    public void HidePerkInfo()
    {
        perkAnimator.SetTrigger("Hide");
    }

    public void TogglePressurePlateInfo(bool toggle, SpellType spellType = SpellType.Harmony)
    {
        if(toggle)
        {
            resourceUseTMP.text = "";
            perkAnimator.SetTrigger("Show");
            if (spellType == SpellType.Harmony)
            {
                perkNameTMP.text = "Harmony";
                perkNameTMP.color = harmonyColor;
                perkFrame.color = harmonyColor;
                perkDescTMP.text = "Progress in Harmony to buff yourself and make life difficult for your enemies. Healing and slowing spells are unique for Harmony.";
            }
            else
            {
                perkNameTMP.text = "Chaos";
                perkNameTMP.color = chaosColor;
                perkFrame.color = chaosColor;
                perkDescTMP.text = "Progress in Chaos to sow destruction and havoc. Bouncing and forbidden spells are unique for Chaos.";

            }
        }
        else
        {
            perkAnimator.SetTrigger("Hide");
        }  
    }

    public void TurnOnBotsUpgradeInfo(List<BotUpgradeInfo> botUpgradeInfo)
    {
        botsUpgradesHolder.gameObject.SetActive(true);
        botsUpgradesTMP.text = "";
 
        for (int i = 0; i < botUpgradeInfo.Count; i++)
        {
            botsUpgradesTMP.text += botUpgradeInfo[i].botname + " => ";
            if(botUpgradeInfo[i].spellType == SpellType.Harmony)
            {
                botsUpgradesTMP.text += DeveloperConsole.AddColorToText(botUpgradeInfo[i].spellname, harmonyColor);
            }
            else
            {
                botsUpgradesTMP.text += DeveloperConsole.AddColorToText(botUpgradeInfo[i].spellname, chaosColor);
            }

            botsUpgradesTMP.text += "\n";
        }
    }

    public void TurnOffBotsUpgradeInfo()
    {
        botsUpgradesHolder.gameObject.SetActive(false);
    }


    public void ChangeRound(string text)
    {
        roundInfo.text = text;
    }
}
