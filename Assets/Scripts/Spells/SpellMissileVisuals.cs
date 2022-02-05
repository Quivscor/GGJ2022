using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMissileVisuals : MonoBehaviour
{
    private SpellType type;
    [SerializeField] private GameObject leftSpellParticles;
    [SerializeField] private GameObject rightSpellParticles;

    [SerializeField] private GameObject leftHitImpactParticles = null;
    [SerializeField] private GameObject rightHitImpactParticles = null;
    [SerializeField] private float hitImpactParticlesLifetime = 1.0f;

    public void Initialize(SpellType type)
    {
        this.type = type;

        if (type == SpellType.Left)
            leftSpellParticles.SetActive(true);
        if (type == SpellType.Right)
            rightSpellParticles.SetActive(true);
    }

    public void CreateHitImpact()
    {
        GameObject createdImpact = null;
        if (type == SpellType.Left)
        {
            if (leftHitImpactParticles != null)
            {
                createdImpact = Instantiate(leftHitImpactParticles, this.transform.position, Quaternion.identity);
            }
        }
        else if(type == SpellType.Right)
        {
            if (rightHitImpactParticles != null)
            {
                createdImpact = Instantiate(rightHitImpactParticles, this.transform.position, Quaternion.identity);
            }
        }

        if(createdImpact != null)
        {
            createdImpact.transform.localScale = this.transform.localScale;
            Destroy(createdImpact, hitImpactParticlesLifetime);
        }
    }
}
