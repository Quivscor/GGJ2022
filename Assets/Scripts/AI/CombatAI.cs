using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{
    public List<Transform> enemyTargets;
    public List<float> enemyThreatGauge;

    private WandController wand;
    private CharacterRotator rotator;

    private void Awake()
    {
        wand = GetComponent<WandController>();
        rotator = GetComponent<CharacterRotator>();
    }

    private void Update()
    {
        wand.ProcessSpell(true, false);
        rotator.LookAt(enemyTargets[0].position);
    }
}
