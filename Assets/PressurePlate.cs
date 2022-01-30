using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private SpellType spellType;

    private bool isPlayerInRange = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = true;
            HudController.Instance.TogglePressEInfo(true, "Press E to choose");
            LevelUp.Instance.TogglePressurePlateInfo(spellType, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HudController.Instance.TogglePressEInfo(false);
            LevelUp.Instance.TogglePressurePlateInfo(spellType, false);

        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LevelUp.Instance.ResourceWasChosen(spellType);
        }
    }
}
