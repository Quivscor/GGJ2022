using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMonument : MonoBehaviour
{
    public int monumentNumber;
    public GameObject detector;
    private bool isPlayerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = true;
            LevelUp.Instance.ShowMonumentInfo(monumentNumber);
            HudController.Instance.TogglePressEInfo(true, "Press E to upgrade");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = false;
            LevelUp.Instance.HideMonumentInfo(monumentNumber);
            HudController.Instance.TogglePressEInfo(false);
        }
    }

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LevelUp.Instance.BuyPerkFromMonument(monumentNumber);
        }
    }
}
