using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMProText = TMPro.TextMeshProUGUI;

public class DialogueBox : MonoBehaviour
{
    [TextArea(4, 10)] public string[] dialogueSequences;
    public int currentSequence;

    [SerializeField] GameObject hud;
    [SerializeField] TMProText text;

    private void Start()
    {
        ShowSequence(0);
    }

    public void ShowSequence(int sequence)
    {
        if (currentSequence > dialogueSequences.Length - 1)
        {
            HideHud();
            return;
        }

        text.text = dialogueSequences[sequence];

        currentSequence++;
    }

    public void HideHud()
    {
        hud.SetActive(false);
    }
}
