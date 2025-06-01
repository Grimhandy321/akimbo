using UnityEngine;
using System.Collections;

public class RailGunProjectile : ProjectileBase
{
    public float dmg = 50f;
    public float velocity = 1000f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private Team team;
    private ushort senderID;
    private Collision collisionInfo;
    private bool collisionEnabled = false;

    public override void Fire(Vector3 position, Vector3 direction, Team teamm, ushort senderID)
    {
        this.team = teamm;
        this.senderID = senderID;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = direction * velocity;

        collisionEnabled = false;
        StartCoroutine(EnableCollisionAfterDelay(0.5f));
    }

    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        collisionEnabled = true;
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
        if (!collisionEnabled) return;

        collisionInfo = collision;
        Detonate();
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }
}
