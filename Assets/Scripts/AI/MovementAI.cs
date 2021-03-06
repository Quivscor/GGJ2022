using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using System.Linq;

public class MovementAI : MonoBehaviour
{
    public List<Transform> enemyTargets;
    public Transform activeTarget;

    [SerializeField] private bool _isUseEnemyDistanceRule = true;
    //This makes character ignore target if it's behind anything that raycast can collide with
    [SerializeField] private bool _isUseWallCancelEnemyDistanceRule = true;
    [SerializeField, Range(0, 1f)] float enemyBias;
    [SerializeField] float enemyDistanceFactor;
    /// <summary>
    /// x is minimal preffered distance, y is maximal preffered distance
    /// </summary>
    [SerializeField] Vector2 prefEnemyDistance;
    [SerializeField, Range(0, 1f)] float targetBias;

    //maybe implement later
    [SerializeField] private bool _isUseAllyDistanceRule = false;
    public List<Transform> allyTargets;
    [SerializeField, Range(0, 1f)] float allyBias;
    [SerializeField] Vector2 prefAllyDistance;

    public Vector3 MoveDir { get; private set; }

    [SerializeField] private bool _isUseOldMoveRule = true;
    [SerializeField, Range(0,1f)] float oldMoveBias;
    [SerializeField] bool _isUseRandomMove;
    [SerializeField] float randomMoveBias;

    [SerializeField] Vector2 biasRange;

    private CharacterStats stats;
    private CharacterMovement charMovement;
    private float colliderRadius;

    [SerializeField] private bool _canDash;
    [SerializeField] float dashThreshold = 30;
    private bool willTryDash = false;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        charMovement = GetComponent<CharacterMovement>();
        colliderRadius = GetComponent<CapsuleCollider>().radius + 0.01f;
    }

    [SerializeField] float aiUpdateFrequency = .5f;
    float updateTimer = 0;

    private void Update()
    {
        if (MatchController.Instance.State != MatchState.ACTIVE || stats.isDead)
        {
            charMovement.ProcessMovement(0, 0);
            return;
        }

        if (updateTimer <= 0)
        {
            MoveDir = GetMoveDir();
            updateTimer = aiUpdateFrequency;
        }
        else
            updateTimer -= Time.deltaTime;

        if (_canDash && willTryDash)
        {
            charMovement.ProcessDash(MoveDir.z, MoveDir.x);
            willTryDash = false;
        }
        else
            charMovement.ProcessMovement(MoveDir.z, MoveDir.x);
    }

    public Vector3 GetMoveDir()
    {
        Vector3 result = Vector3.zero;

        //serialize movement weights
        Vector3[] weightVectors = GetSteeringBehaviorWeights();
        float[] weights = new float[8];

        //stay away from opponents rule
        GetOpponentsRuleMovement(weightVectors, ref weights);
        //consider allies rule
        GetAlliesRuleMovement(weightVectors, ref weights);
        //stay away from arena walls rule
        GetWallsRuleMovement(weightVectors, ref weights);
        //dodge bullets rule
        GetDodgeBulletsRuleMovement(weightVectors, ref weights);
        //keep old move direction rule
        GetOldMoveRuleMovement(weightVectors, ref weights);

        for(int i = 0; i < weights.Length; i++)
        {
            if(Mathf.Abs(weights[i]) > biasRange.x && Mathf.Abs(weights[i]) < biasRange.y)
            {
                result += weightVectors[i] * weights[i];
            }
            try
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(this.transform.position, weightVectors[i] * weights[i]);
            }
            //i only use this so i can use this function in OnDrawGizmos
            catch (Exception) {}
        }
        if(_isUseRandomMove)
        {
            result += Random.insideUnitSphere * randomMoveBias;
        }
        if (result.magnitude > dashThreshold)
            willTryDash = true;

        return result.normalized;
    }

    private void GetOpponentsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        if (!_isUseEnemyDistanceRule)
            return;

        foreach (Transform t in enemyTargets)
        {
            if (t == null)
                continue;

            //vector towards target
            Vector3 unitDistance = t.position - this.transform.position;

            //consider main opponent more than others
            float opponentBias = enemyBias;
            if (t == activeTarget)
                opponentBias = targetBias;

            //if something is between objects
            RaycastHit hitinfo;
            if(_isUseWallCancelEnemyDistanceRule)
            {
                if (Physics.Linecast(this.transform.position + Vector3.up, t.position + Vector3.up, out hitinfo))
                {
                    if (hitinfo.transform != t)
                        continue;
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                float weightDelta = Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                        VectorCast.CastVector3ToVector2(unitDistance.normalized)) * 
                        (opponentBias + ((1/unitDistance.magnitude) * enemyDistanceFactor));
                //if enemy too far from target, walk up to it
                if (unitDistance.magnitude > prefEnemyDistance.y)
                {
                    weights[i] += weightDelta;
                }
                //if not, move away from them
                else if(unitDistance.magnitude < prefEnemyDistance.x)
                {
                    weights[i] -= weightDelta;
                }
            }
        }
    }

    private void GetAlliesRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        if (!_isUseAllyDistanceRule)
            return;

        foreach (Transform t in allyTargets)
        {
            if (t == null)
                continue;

            //vector towards target
            Vector3 unitDistance = t.position - this.transform.position;

            for (int i = 0; i < weights.Length; i++)
            {
                float weightDelta = Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                        VectorCast.CastVector3ToVector2(unitDistance.normalized)) *
                        allyBias;
                //if enemy too far from target, walk up to it
                if (unitDistance.magnitude > prefAllyDistance.y)
                {
                    weights[i] += weightDelta;
                }
                //if not, move away from them
                else if (unitDistance.magnitude < prefAllyDistance.x)
                {
                    weights[i] -= weightDelta;
                }
            }
        }
    }

    [SerializeField] private bool _isUseWallDistanceRule = true;
    [SerializeField] float wallRaycastDistance;
    [SerializeField] LayerMask wallLayerMask;
    [SerializeField] float wallWeightBonus;
    [SerializeField, Range(0, 5f)] float wallBias;
    [SerializeField] float wallDistanceFactor;

    private void GetWallsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        if (!_isUseWallDistanceRule)
            return;

        List<RaycastHit> hits = new List<RaycastHit>();
        foreach(Vector3 dir in vectors)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, dir, out hit, wallRaycastDistance, wallLayerMask);
            hits.Add(hit);
        }

        float[] testWeights = new float[8];
        int testCounter = 0;
        for(int i = 0; i < weights.Length; i++)
        {
            if(hits[i].collider == null)
            {
                //if wall not detected, nudge slightly in that direction
                testWeights[i] += wallWeightBonus;
                testCounter++;
            }
            else
            {
                //if wall detected, you don't want to go there, the closer you are the more you don't want to go there
                weights[i] -= (wallRaycastDistance / Vector3.Distance(this.transform.position, hits[i].point)) * wallBias;
            }
        }

        if(testCounter < weights.Length)
        {
            //not all directions returned no wall, so apply nudge in those directions
            for(int i = 0; i < weights.Length; i++)
            {
                weights[i] += testWeights[i];
            }
        }
    }

    [SerializeField] private bool _isUseBulletDodgeRule = true;
    [SerializeField] float bulletDetectionRange;
    [SerializeField] float bulletBias;
    [SerializeField] string bulletTag;
    private void GetDodgeBulletsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        if (!_isUseBulletDodgeRule)
            return;

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, bulletDetectionRange);

        foreach(Collider col in colliders)
        {
            if(col.CompareTag(bulletTag))
            {
                SpellMissile bullet;
                if(col.TryGetComponent(out bullet))
                {
                    if (bullet.GetParentTransform() == this.transform)
                        continue;
                }

                Vector3 unitDistance = Vector3.Normalize(col.transform.position - this.transform.position);

                for (int i = 0; i < weights.Length; i++)
                {
                    //dodge bullets
                    weights[i] -= Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                        VectorCast.CastVector3ToVector2(unitDistance)) * bulletBias;
                }
            }
        }
    }

    float[] oldWeights = new float[8];
    private void GetOldMoveRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        if (!_isUseOldMoveRule)
            return;

        for (int i = 0; i < weights.Length; i++)
        {
            if(weights[i] != 0)
                weights[i] += oldWeights[i] * oldMoveBias;
        }
        oldWeights = weights;
    }

    private Vector3[] GetSteeringBehaviorWeights()
    {
        Vector3[] weightVectors = new Vector3[8] {Vector3.forward, Vector3.RotateTowards(Vector3.forward, Vector3.right, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.right, Vector3.RotateTowards(Vector3.right, Vector3.back, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.back, Vector3.RotateTowards(Vector3.back, Vector3.left, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.left, Vector3.RotateTowards(Vector3.left, Vector3.forward, 45.0f * Mathf.Deg2Rad, 0)};
        return weightVectors;
    }

    [SerializeField] bool debugDrawRange = true;
    private void OnDrawGizmos()
    {
        //GetMoveDir();

        if(debugDrawRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, prefEnemyDistance.x);
            Gizmos.DrawWireSphere(this.transform.position, prefEnemyDistance.y);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, wallRaycastDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, bulletDetectionRange);
        }
    }
}
