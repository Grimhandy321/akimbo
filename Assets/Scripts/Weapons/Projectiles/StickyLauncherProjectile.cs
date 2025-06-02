using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyLauncherProjectile : ProjectileBase
{
    public float dmg = 10f;
    public float velocity = 1000f;
    public float explosionDelay = 2f;
    public float explosionRadius = 5f;

    private Rigidbody rb;
    private Team team;
    private ushort senderID;
    private bool hasStuck = false;

    public override void Fire(Vector3 position, Vector3 direction, Team teamm, ushort senderID)
    {
        this.team = teamm;
        this.senderID = senderID;

        float forwardOffset = 1.5f;
        Vector3 spawnPosition = position + direction.normalized * forwardOffset;

        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(direction);

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = direction.normalized * velocity;
    }

    public override void Detonate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            GameObject otherObject = hit.gameObject;
            ITargetable target = otherObject.GetComponent<ITargetable>();
            CoinManager coinManager = otherObject.GetComponent<CoinManager>();

            if (target != null)
                target.Damage(team, dmg, senderID);

            if (coinManager != null)
                coinManager.HitByHitScan(dmg, team, senderID);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasStuck) return;

        GameObject otherObject = collision.collider.gameObject;

        CoinManager coinManager = otherObject.GetComponent<CoinManager>();
        if (coinManager != null)
        {
            coinManager.HitByProjectile(team,gameObject);
            Destroy(gameObject);
            return;
        }

        hasStuck = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = collision.contacts[0].point;
        transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        transform.SetParent(collision.transform);

        StartCoroutine(DelayedExplosion());
    }

    private IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Detonate();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}



