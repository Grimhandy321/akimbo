using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 30f;

    public override void Fire(Transform firePoint, Team shooterTeam)
    {
        if (!CanFire() || projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.Initialize(damage, projectileSpeed, shooterTeam, canFriendlyFire);

        ResetFireCooldown();
    }
}
