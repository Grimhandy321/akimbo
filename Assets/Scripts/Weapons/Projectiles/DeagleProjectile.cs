using UnityEngine;

public class DeagleProjectile : ProjectileBase
{
    public float damage = 50f;
    public float range = 5000f;

    public override void Detonate()
    {
        // Not used for hitscan but required by abstract class
    }

    public override void Fire(Team team)
    {
        var cam = Camera.main;
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, range))
        {
            ITargetable target = hitInfo.transform.GetComponent<ITargetable>();
            CoinManager coinManager = hitInfo.transform.GetComponent<CoinManager>();

            if (target != null)
            {
                target.Damage(team, damage);
            }
            if (coinManager != null)
            {
                coinManager.HitByHitScan(damage, team);
            }
        }
    }
}
