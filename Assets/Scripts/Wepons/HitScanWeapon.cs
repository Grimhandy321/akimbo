using UnityEngine;

public class HitScanWeapon : Weapon
{
    public float range = 100f;

    public override void Fire(Transform firePoint, Team shooterTeam)
    {
        if (!CanFire()) return;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            var target = null; // togo hp srit

            if (target != null && (target.PlayerTeam != shooterTeam || canFriendlyFire))
            {
                target.TakeDamage(damage);
            }

            Debug.Log($"{weaponName} hit {hit.collider.name}");
        }

        ResetFireCooldown();
    }
}
