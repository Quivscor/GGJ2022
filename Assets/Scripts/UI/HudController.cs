using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HudController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image resourceBar;
    [SerializeField]
    private Image healthBar;
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
    [SerializeField]
    private Color harmonyColor;
    [SerializeField]
    private Color chaosColor;

    [Header("Press E Info")]
    [SerializeField]
    private TextMeshProUGUI pressEInfoTMP;
    [SerializeField]
    private GameObject pressEInfo;

    private bool perkInfoVisible = false;

    public static HudController Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        pressEInfo.SetActive(toggle);
    }
    public void HidePerkInfo()
    {
        perkAnimator.SetTrigger("Hide");
    }
}
