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
    [SerializeField] TMProText numberOfHints;

    private void Start()
    {
        currentSequence = -1;
        NextSequence(); 
    }

    public void NextSequence()
    {
        if (currentSequence + 1 > dialogueSequences.Length - 1)
        {
            HideHud();
            return;
        }

        currentSequence++;
        text.text = dialogueSequences[currentSequence];
        numberOfHints.text = currentSequence + " / " + dialogueSequences.Length;
        
    }

    public void PreviousSequence()
    {
        if (currentSequence - 1 < 0)
        {
            return;
        }

        currentSequence--;
        text.text = dialogueSequences[currentSequence];
        numberOfHints.text = currentSequence + " / " + dialogueSequences.Length;

    }
    public void HideHud()
    {
        hud.SetActive(false);
    }
}
