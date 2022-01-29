using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingAI : MonoBehaviour
{
    List<Target> enemyTargets;
    List<Target> allyTargets;

    private MovementAI movement;
    private CombatAI combat;

    private CharacterHolder characterHolder;

    [SerializeField] float aiUpdateFrequency;
    float currentTime;

    [SerializeField] float threatDecayOverTime;
    [SerializeField] float threatIncreasePerDmg;
    [SerializeField] float initThreatValue;

    private void Awake()
    {
        enemyTargets = new List<Target>();

        movement = GetComponent<MovementAI>();
        combat = GetComponent<CombatAI>();

        characterHolder = FindObjectOfType<CharacterHolder>();
    }

    private void Update()
    {
        if (currentTime <= 0)
        {
            SelectPrimaryTarget(EvaluateTargets());
            currentTime = aiUpdateFrequency;
        }
        else
            currentTime -= Time.deltaTime;

        foreach(Target t in enemyTargets)
        {
            UpdateThreat(t);
        }
    }

    public Target GetTargetFromTransform(Transform t)
    {
        Target target = enemyTargets.Find((x) => x.transform == t);
        if (target == null)
            target = new Target(t, initThreatValue);
        return target;
    }

    public void UpdateThreat(Target target, float dmg = 0)
    {
        if(dmg > 0)
        {
            target.threatGauge += dmg * threatIncreasePerDmg;
        }

        target.threatGauge -= Time.deltaTime * threatDecayOverTime;

        if(target.threatGauge <= 0)
        {
            enemyTargets.Remove(target);
            RemoveEnemy(target);
        }
    }

    private Target EvaluateTargets()
    {
        Target result = new Target();

        foreach(Target t in enemyTargets)
        {
            if (t.threatGauge > result.threatGauge)
                result = t;
        }

        return result;
    }

    private void SelectPrimaryTarget(Target target)
    {
        movement.activeTarget = target.transform;
        combat.activeTarget = target.transform;
    }

    private void RemoveEnemy(Target enemy)
    {
        movement.enemyTargets.Remove(enemy.transform);
    }

    private void AddEnemy(Target enemy)
    {
        movement.enemyTargets.Add(enemy.transform);
    }
}

[System.Serializable]
public class Target
{
    public Transform transform;
    public float threatGauge;

    public Target()
    {
        transform = null;
        threatGauge = 0;
    }

    public Target(Transform transform, float threat)
    {
        this.transform = transform;
        threatGauge = threat;
    }
}