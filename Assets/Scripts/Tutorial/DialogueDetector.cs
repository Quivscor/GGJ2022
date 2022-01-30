using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDetector : MonoBehaviour
{
    bool isInRange = false;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] GameObject continuePopup;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isInRange = true;
            continuePopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isInRange = false;
            continuePopup.SetActive(false);
        } 
    }

    private void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueBox.ShowSequence(dialogueBox.currentSequence);
        }
    }
}
