using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed = 50f;
    protected Team shooterTeam;
    protected bool canFriendlyFire;
    public float damage;

    public void Launch(Vector3 direction, Team shooterTeam, float damage, bool canFriendlyFire)
    {
        this.shooterTeam = shooterTeam;
        this.damage = damage;
        this.canFriendlyFire = canFriendlyFire;
        OnLaunch(direction);
    }
    protected abstract void OnLaunch(Vector3 direction);

    protected void OnTriggerEnter(Collider other)
    {
        PlayerController hitPlayer = other.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            hitPlayer.TakeDamage(damage, shooterTeam, canFriendlyFire);
        }

        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CoinManager>(out var coin)) // catch coin in mid air 
        {
            coin.HitByProjectile(this, shooterTeam);
            return;
        }
    }
}
