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
    private CharacterStats stats;

    [SerializeField] float aiUpdateFrequency;
    float aiCurrentTime;
    [SerializeField] float areaScanFrequency;
    float areaScanCurrentTime;

    [SerializeField] float threatDecayOverTime;
    [SerializeField] float threatIncreasePerDmg;
    [SerializeField] float initThreatValue;
    [SerializeField] float areaScanRadius;
    [SerializeField] float threatDistanceMod;

    private void Awake()
    {
        enemyTargets = new List<Target>();

        movement = GetComponent<MovementAI>();
        combat = GetComponent<CombatAI>();

        characterHolder = FindObjectOfType<CharacterHolder>();
        stats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (MatchController.Instance.State != MatchState.ACTIVE || stats.isDead)
            return;

        if (aiCurrentTime <= 0)
        {
            SelectPrimaryTarget(EvaluateTargets());
            aiCurrentTime = aiUpdateFrequency;
        }
        else
            aiCurrentTime -= Time.deltaTime;

        if (areaScanCurrentTime <= 0)
        {
            CheckEnemiesInArea();
            areaScanCurrentTime = areaScanFrequency;
        }
        else
            areaScanCurrentTime -= Time.deltaTime;

        foreach(Target t in enemyTargets)
        {
            UpdateThreat(t);
        }
    }

    public void CheckEnemiesInArea()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, areaScanRadius);
        foreach(Collider col in colliders)
        {
            if(col.TryGetComponent(out CharacterStats stats))
            {
                //you're not your own enemy
                if (col.transform.root == this.transform)
                    continue;

                Target t = GetTargetFromTransform(col.transform);
                t.threatGauge += areaScanRadius / Vector3.Distance(this.transform.position, t.transform.position) * threatDistanceMod;
            }
        }
    }

    public Target GetTargetFromTransform(Transform t)
    {
        Target target = enemyTargets.Find((x) => x.transform == t);
        if (target == null)
        {
            target = new Target(t, initThreatValue);
            AddEnemy(target);
        }
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
        if(!enemyTargets.Contains(enemy))
            enemyTargets.Add(enemy);

        if(!movement.enemyTargets.Contains(enemy.transform))
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