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
    private Collision collisionInfo;

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
        if (collisionInfo == null) return;
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            GameObject otherObject = contact.otherCollider.gameObject;
            ITargetable target = otherObject.GetComponent<ITargetable>();
            CoinManager coinManager = otherObject.GetComponent<CoinManager>();

            if (target != null)
                target.Damage(team, dmg, senderID);

            if (coinManager != null)
                coinManager.HitByHitScan(dmg, team, senderID);
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionInfo = collision;
        Detonate();
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }
}
