using UnityEngine;
using System.Collections;

public class StickyGrenade : ProjectileBase
{
    public float dmg = 50f;
    public float fuseTime = 3f;
    public float colliderEnableDelay = 0.1f;
    public float velocity = 1000f;

    private Rigidbody rb;
    private Collider col;
    private Team team;
    private ushort senderID;
    private bool hasStuck = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public override void Fire(Vector3 position, Vector3 direction, Team teamm, ushort senderID)
    {
        this.team = teamm;
        this.senderID = senderID;
        transform.position = position;
        rb.velocity = direction.normalized * 10f;

        rb.velocity = direction.normalized * velocity;
        if (col != null) col.enabled = false;
        StartCoroutine(EnableColliderAfterDelay(colliderEnableDelay));
        Invoke(nameof(Detonate), fuseTime);
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null)
            col.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasStuck) return;
        hasStuck = true;
        rb.isKinematic = true;
        transform.SetParent(collision.collider.transform, true);
    }

    public override void Detonate()
    {
        if (hasStuck == false && col != null)
            col.enabled = true; 
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hits)
        {
            ITargetable target = hit.GetComponent<ITargetable>();
            if (target != null)
                target.Damage(team, dmg, senderID);
        }
        Destroy(gameObject);
    }
}
