using UnityEngine;

public class HitScanWeapon : Weapon
{
    public float range = 100f;

    public override void Fire(Transform firePoint, Team shooterTeam)
    {
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, range))
        {
            Debug.Log($"{gameObject.name} hit {hit.collider.name}");

            // Check if we hit a TeleportObject
            if (hit.collider.TryGetComponent<CoinManager>(out var teleportObject))
            {
                teleportObject.HitByHitScan(damage, shooterTeam);
            }

            // Check if we hit a player
            if (hit.collider.TryGetComponent<PlayerController>(out var player))
            {
                if (player.PlayerTeam != shooterTeam)
                {
                    player.TakeDamage(damage);
                }
            }
        }
        ResetFireCooldown();

    }
}
