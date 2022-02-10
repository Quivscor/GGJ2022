using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class TargetingAI : MonoBehaviour
{
    [SerializeField] private AITeam _team;
    public AITeam Team => _team;

    List<Target> enemyTargets;
    List<Target> allyTargets;

    private MovementAI movement;
    private CombatAI combat;

    private CharacterHolder characterHolder;
    private CharacterStats stats;

    [SerializeField] float aiUpdateFrequency;
    private Timer aiUpdateTimer;

    [SerializeField] float areaScanFrequency;
    private Timer areaScanTimer;

    [SerializeField] float threatDecayOverTime;
    [SerializeField] float threatIncreasePerDmg;
    [SerializeField] float initThreatValue;
    [SerializeField] float areaScanRadius;
    [SerializeField] float threatDistanceMod;

    private void Awake()
    {
        enemyTargets = new List<Target>();
        allyTargets = new List<Target>();

        movement = GetComponent<MovementAI>();
        combat = GetComponent<CombatAI>();

        characterHolder = FindObjectOfType<CharacterHolder>();
        stats = GetComponent<CharacterStats>();
        stats.damageReceived += UpdateThreat;

        aiUpdateTimer = new Timer(aiUpdateFrequency, SelectPrimaryTarget);
        areaScanTimer = new Timer(areaScanFrequency, CheckCharactersInArea);
    }

    private void Update()
    {
        if (MatchController.Instance.State != MatchState.ACTIVE || stats.isDead)
            return;

        SanityTargetLists();

        float time = Time.deltaTime;

        aiUpdateTimer.Update(time);
        areaScanTimer.Update(time);

        foreach(Target t in new List<Target>(enemyTargets))
        {
            if (t.stats.isDead)
            {
                RemoveEnemy(t);
                enemyTargets.Remove(t);
            }
                
            UpdateThreat(t);
        }
    }

    private void SanityTargetLists()
    {
        List<Target> targetsCopy = new List<Target>(enemyTargets);
        foreach(Target t in targetsCopy)
        {
            if (t.stats == null)
                enemyTargets.Remove(t);
        }

        targetsCopy = new List<Target>(allyTargets);
        foreach(Target t in targetsCopy)
        {
            if (t.stats == null)
                allyTargets.Remove(t);
        }
    }

    public void ClearEnemyTargets()
    {
        enemyTargets.Clear();
    }

    public void ClearAllyTargets()
    {
        allyTargets.Clear();
    }

    public void CheckCharactersInArea()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, areaScanRadius);
        foreach(Collider col in colliders)
        {
            if(col.TryGetComponent(out CharacterStats stats))
            {
                if (stats.isDead)
                    continue;

                //you're not your own enemy
                if (col.transform.root == this.transform)
                    continue;

                //allies are not enemies
                if (allyTargets.Contains(GetAllyTargetFromTransform(col.transform)))
                    continue;

                if(col.TryGetComponent(out TargetingAI ai))
                {
                    if(ai.Team == this.Team && ai.Team != AITeam.NOTEAM)
                    {
                        CreateTargetFromTransform(col.transform, false);
                        continue;
                    }
                }

                //skip if not in line of sight
                RaycastHit hitinfo;
                if (Physics.Linecast(this.transform.position + Vector3.up, col.transform.position + Vector3.up, out hitinfo))
                {
                    if (hitinfo.transform != col.transform)
                        continue;
                }

                Target t = GetEnemyTargetFromTransform(col.transform);
                if (t != null)
                    t.threatGauge += areaScanRadius / Vector3.Distance(this.transform.position, t.transform.position) * threatDistanceMod;
                else
                    CreateTargetFromTransform(col.transform, true);
            }
        }

        areaScanTimer.RestartTimer();
    }

    public Target GetEnemyTargetFromTransform(Transform t)
    {
        if (t == this.transform)
            return null;

        Target target;
        target = enemyTargets.Find((x) => x.transform == t);

        return target;
    }

    public Target GetAllyTargetFromTransform(Transform t)
    {
        if (t == this.transform)
            return null;

        Target target;
        target = allyTargets.Find((x) => x.transform == t);

        return target;
    }

    public Target CreateTargetFromTransform(Transform t, bool isEnemy)
    {
        Target target = new Target(t, initThreatValue);
        if (isEnemy)
            AddEnemy(target);
        else
            AddAlly(target);

        return target;
    }

    public void UpdateThreat(Target target, float dmg = 0)
    {
        if (target == null)
            return;

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

    public void UpdateThreat(Transform transform, float damage)
    {
        UpdateThreat(GetEnemyTargetFromTransform(transform), damage);
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

    private void SelectPrimaryTarget()
    {
        Target t = EvaluateTargets();

        SelectPrimaryTarget(t);
        aiUpdateTimer.RestartTimer();
    }

    private void RemoveEnemy(Target enemy)
    {
        movement.enemyTargets.Remove(enemy.transform);
    }

    private void RemoveAlly(Target ally)
    {
        movement.allyTargets.Remove(ally.transform);
    }

    private void AddEnemy(Target enemy)
    {
        if(!enemyTargets.Contains(enemy))
            enemyTargets.Add(enemy);

        if(!movement.enemyTargets.Contains(enemy.transform))
            movement.enemyTargets.Add(enemy.transform);
    }

    private void AddAlly(Target ally)
    {
        if (!allyTargets.Contains(ally))
            allyTargets.Add(ally);

        if (!movement.allyTargets.Contains(ally.transform))
            movement.allyTargets.Add(ally.transform);
    }
}

[System.Serializable]
public class Target
{
    public Transform transform;
    public CharacterStats stats;
    public float threatGauge;

    public Target()
    {
        transform = null;
        threatGauge = 0;
    }

    public Target(Transform transform, float threat)
    {
        this.transform = transform;
        stats = transform.GetComponent<CharacterStats>();
        threatGauge = threat;
    }
}