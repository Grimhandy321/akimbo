using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed = 50f;
    protected Team shooterTeam;
    protected bool canFriendlyFire;
    public float damage;

    [Header("Optional Settings")]
    public float lifetime = 5f;

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    public void Launch(Vector3 direction, Team shooterTeam, float damage, bool canFriendlyFire)
    {
        this.shooterTeam = shooterTeam;
        this.damage = damage;
        this.canFriendlyFire = canFriendlyFire;

        OnLaunch(direction);
    }
    protected abstract void OnLaunch(Vector3 direction);

    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerController hitPlayer = other.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            hitPlayer.TakeDamage(damage, shooterTeam, canFriendlyFire);
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CoinManager>(out var coin))
        {
            coin.HitByProjectile(this, shooterTeam);
        }

        Destroy(gameObject);
    }
}
