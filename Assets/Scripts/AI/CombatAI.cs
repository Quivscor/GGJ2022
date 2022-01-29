using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{
    public Transform activeTarget;

    private WandController wand;
    private CharacterRotator rotator;

    private void Awake()
    {
        wand = GetComponent<WandController>();
        rotator = GetComponent<CharacterRotator>();
    }

    private void Update()
    {
        if (activeTarget == null)
            return;

        wand.ProcessSpell(true, false);
        rotator.LookAt(activeTarget.position);
    }
}
