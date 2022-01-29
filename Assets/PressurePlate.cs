using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private SpellType spellType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.root.CompareTag("Player"))
        {
            LevelUp.Instance.ResourceWasChosen(spellType);
        }
    }
}
