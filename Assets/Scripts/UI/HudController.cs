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
    private Animator perkAnimator;
    [SerializeField]
    private Image perkFrame;

    private bool perkInfoVisible = false;

    public static HudController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    
    public void ShowPerkInfo(string perkName, string perkDesc)
    {
        perkNameTMP.text = perkName;
        perkDescTMP.text = perkDesc;
        perkAnimator.SetTrigger("Show");
    }

    public void HidePerkInfo()
    {
        perkAnimator.SetTrigger("Hide");
    }
}
