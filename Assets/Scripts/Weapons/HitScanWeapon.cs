using UnityEngine;

public class HitScanWeapon : Weapon
{
    public float range = 100f;

    public override void Fire(Transform firePoint, Team shooterTeam)
    {
        if (!CanFire()) return;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, range))
        {
            Debug.Log($"{gameObject.name} hit {hit.collider.name}");

            if (hit.collider.TryGetComponent<CoinManager>(out var teleportObject))
            {
                teleportObject.HitByHitScan(damage, shooterTeam);
            }

            if (hit.collider.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(damage, shooterTeam, canFriendlyFire);
            }
        }

        PlayFireSound();
        ResetFireCooldown();
    }
}
