using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class RoundCounter : MonoBehaviour
{
    TMProText text;

    private void Awake()
    {
        text = GetComponent<TMProText>();
    }

    public void UpdateText(float round)
    {
        text.text = "Round #" + round;
    }
}
