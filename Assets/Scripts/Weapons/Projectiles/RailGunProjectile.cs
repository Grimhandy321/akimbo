using UnityEngine;

public class RailGunProjectile : ProjectileBase
{
    public float dmg = 50f;
    public float velocity = 1000f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private Team team;
    private ushort senderID;
    private Collision collisionInfo;

    public override void Fire(Vector3 position, Vector3 direction, Team teamm,ushort senderID)
    {
        this.team = team;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
        this.senderID = senderID;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = direction * velocity;
        Destroy(gameObject, lifeTime);
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
                coinManager.HitByHitScan(dmg, team,senderID);
        }

        Destroy(gameObject);
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
