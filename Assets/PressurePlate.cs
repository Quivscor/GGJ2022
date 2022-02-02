using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private SpellType spellType;

    private bool activated = false;


    private void OnEnable()
    {
        activated = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            activated = false;
            HudController.Instance.TogglePressEInfo(false, "Press E to choose");
            LevelUp.Instance.TogglePressurePlateInfo(spellType, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("Player"))
        {
            if(!activated)
            {
                activated = true;
                HudController.Instance.TogglePressEInfo(true, "Press E to choose");
                LevelUp.Instance.TogglePressurePlateInfo(spellType, true);
            }
        }
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.E) && activated)
        {
            LevelUp.Instance.ResourceWasChosen(spellType);
        }
    }
}
