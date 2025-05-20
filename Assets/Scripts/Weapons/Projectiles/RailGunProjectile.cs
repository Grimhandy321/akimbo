using UnityEngine;

public class RailGunProjectile : ProjectileBase
{
    public float dmg = 50f;
    public float velocity = 1000f;
    public float lifeTime = 5f;
    private Rigidbody rb;
    private Team team;
    private Collision collisionInfo;

    public override void Detonate()
    {
        if (collisionInfo == null) return;

        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            GameObject otherObject = contact.otherCollider.gameObject;
            ITargetable target = otherObject.GetComponent<ITargetable>();
            CoinManager coinManager = otherObject.GetComponent<CoinManager>();

            if (target != null)
            {
                target.Damage(team, dmg);
            }
            if (coinManager != null)
            {
                coinManager.HitByHitScan(dmg, team);
            }
        }
        Destroy(gameObject);
    }

    public override void Fire(Team team)
    {
        this.team = team;

        var cam = Camera.main;
        Vector3 position = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = direction * velocity;
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
