using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HudController : MonoBehaviour
{
    [SerializeField]
    private Image resourceBar;

    [SerializeField]
    private Image healthBar;

    public static HudController instance;
    public static HudController Instance
    {
        get
        {
            if (instance == null)
                instance = new HudController();

            return instance;
        }
    }

    public void UpdateResourceBar(float fillAmount)
    {
        resourceBar.fillAmount = fillAmount;
    }

    public void UpdateHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
