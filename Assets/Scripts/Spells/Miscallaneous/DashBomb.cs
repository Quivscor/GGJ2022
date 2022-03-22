using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrunkenDwarves;

public class DashBomb : DashObject
{
    [SerializeField] private float timeBeforeExplosion;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    [SerializeField] private GameObject explosionEffect;
    private Timer explosionTimer;
    private string _ownerTag;

    public override void Initialize(Transform owner)
    {
        base.Initialize(owner);

        _ownerTag = owner.tag;
    }

    private void Start()
    {
        explosionTimer = new Timer(timeBeforeExplosion, Explode);
    }

    private void Update()
    {
        explosionTimer.Update(Time.deltaTime);
    }

    private void Explode()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach(Collider col in cols)
        {
            if (col.CompareTag(_ownerTag))
                continue;
            if(col.TryGetComponent(out CharacterStats character))
            {
                character.DealDamage(explosionDamage);
            }
        }
        explosionEffect.SetActive(true);
        Destroy(this.gameObject, .4f);
    }
}
