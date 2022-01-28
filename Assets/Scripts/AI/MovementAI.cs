using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAI : MonoBehaviour
{
    public List<Transform> enemyTargets;
    public Transform activeTarget;

    [SerializeField, Range(0, 5f)] float enemyBias;
    [SerializeField, Range(0, 5f)] float targetBias;

    public List<Transform> allyTargets;

    [SerializeField, Range(0, 5f)] float allyBias;

    public Vector3 moveDir;
    [SerializeField, Range(0,5f)] float oldMoveBias;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetMoveDir()
    {
        Vector3 result = Vector3.zero;
        //init with some randomness
        result = Random.insideUnitCircle;
        //serialize movement weights
        Vector3[] weightVectors = GetSteeringBehaviorWeights();
        float[] weights = new float[8];

        //stay away from opponents rule
        GetOpponentsRuleMovement(weightVectors, ref weights);
        //stay away from arena walls rule
        GetWallsRuleMovement(weightVectors, ref weights);
        //dodge bullets rule
        GetDodgeBulletsRuleMovement(weightVectors, ref weights);
        //keep old move direction rule
        GetOldMoveRuleMovement(weightVectors, ref weights);

        return moveDir = result;
    }

    private void GetOpponentsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        foreach(Transform t in enemyTargets)
        {
            Vector3 unitDistance = Vector3.Normalize(t.position - this.transform.position);
            for(int i = 0; i < weights.Length; i++)
            {
                weights[i] = Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                    VectorCast.CastVector3ToVector2(unitDistance)) * enemyBias;
            }
        }
    }

    [SerializeField] float wallRaycastDistance;
    [SerializeField] LayerMask wallLayerMask;
    [SerializeField, Range(0, 5f)] float wallBias;

    private void GetWallsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        foreach(Vector3 dir in vectors)
        {
            RaycastHit hit;
            if(Physics.Raycast(this.transform.position, dir, out hit, wallRaycastDistance, wallLayerMask))
            {
                hits.Add(hit);
            }
        }

        foreach(RaycastHit hit in hits)
        {
            Vector3 unitDistance = Vector3.Normalize(hit.point - this.transform.position);
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                    VectorCast.CastVector3ToVector2(unitDistance)) * wallBias;
            }
        }
    }

    [SerializeField] float bulletDetectionRange;
    [SerializeField] float bulletBias;
    private void GetDodgeBulletsRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, bulletDetectionRange);

        foreach(Collider col in colliders)
        {
            //check collider tag

            Vector3 unitDistance = Vector3.Normalize(col.transform.position - this.transform.position);

            for(int i = 0; i < weights.Length; i++)
            {
                weights[i] += Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                    VectorCast.CastVector3ToVector2(unitDistance)) * bulletBias;
            }
        }
    }

    private void GetOldMoveRuleMovement(Vector3[] vectors, ref float[] weights)
    {
        for(int i = 0; i < weights.Length; i++)
        {
            weights[i] += Vector2.Dot(VectorCast.CastVector3ToVector2(vectors[i]),
                    VectorCast.CastVector3ToVector2(moveDir)) * oldMoveBias;
        }
    }

    private Vector3[] GetSteeringBehaviorWeights()
    {
        Vector3[] weightVectors = new Vector3[8] {Vector3.forward, Vector3.RotateTowards(Vector3.forward, Vector3.right, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.right, Vector3.RotateTowards(Vector3.right, Vector3.back, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.back, Vector3.RotateTowards(Vector3.back, Vector3.left, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.left, Vector3.RotateTowards(Vector3.left, Vector3.forward, 45.0f * Mathf.Deg2Rad, 0)};
        return weightVectors;
    }
}
