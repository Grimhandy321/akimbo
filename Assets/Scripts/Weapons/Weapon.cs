using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName = "Weapon";
    public float damage = 10f;
    public float fireRate = 0.5f; // Shots per second
    public bool canFriendlyFire = false;

    protected float nextFireTime;

    public abstract void Fire(Transform firePoint, Team shooterTeam);

    protected bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void ResetFireCooldown()
    {
        nextFireTime = Time.time + 1f / fireRate;
    }
}
