using UnityEngine;

public class DeagleProjectile : ProjectileBase
{
    public float damage = 50f;
    public float range = 5000f;

    public override void Fire(Vector3 origin, Vector3 direction, Team team, ushort senderId)
    {
        if (hasDetonated) return;
        hasDetonated = true;

        Team = team;
        SenderId = senderId;

        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, range))
        {
            ITargetable target = hitInfo.transform.GetComponent<ITargetable>();
            CoinManager coinManager = hitInfo.transform.GetComponent<CoinManager>();

            if (target != null)
                target.Damage(team, damage, senderId);

            if (coinManager != null)
                coinManager.HitByHitScan(damage, team, senderId);
        }

        Destroy(gameObject);
    }
}