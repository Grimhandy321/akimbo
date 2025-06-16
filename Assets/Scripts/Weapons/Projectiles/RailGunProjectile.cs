using UnityEngine;
using System.Collections;

public class RailGunProjectile : ProjectileBase
{
    public float dmg = 10f;
    public float velocity = 10000f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private Team team;
    private ushort senderID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("RailGunProjectile: Rigidbody is missing.");
    }

    public override void Fire(Vector3 position, Vector3 direction, Team teamm, ushort senderID)
    {
        this.team = teamm;
        this.senderID = senderID;

        Vector3 spawnPos = position + direction.normalized * 1.5f;
        transform.position = spawnPos;
        transform.rotation = Quaternion.LookRotation(direction);

        rb.velocity = direction.normalized * velocity;

        // Auto-detonate
        Invoke(nameof(Detonate), lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasDetonated) return;

        hasDetonated = true;
        DetonateInternal(collision);
    }

    private void DetonateInternal(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject hitObject = contact.otherCollider.gameObject;
            ITargetable target = hitObject.GetComponent<ITargetable>();
            CoinManager coinManager = hitObject.GetComponent<CoinManager>();

            if (target != null)
                target.Damage(team, dmg, senderID);

            if (coinManager != null)
                coinManager.HitByHitScan(dmg, team, senderID);
        }

        Destroy(gameObject);
    }

    public override void Detonate()
    {
        if (hasDetonated) return;
        hasDetonated = true;
        Destroy(gameObject);
    }
}