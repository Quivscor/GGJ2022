using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMonument : MonoBehaviour
{
    public int monumentNumber;

    private bool isPlayerInRange;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = true;
            LevelUp.Instance.ShowMonumentInfo(monumentNumber);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = false;
            LevelUp.Instance.HideMonumentInfo(monumentNumber);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}
