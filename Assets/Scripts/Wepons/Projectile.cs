using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private Team shooterTeam;
    private bool canFriendlyFire;

    public void Initialize(float damage, float speed, Team team, bool canFriendlyFire)
    {
        this.damage = damage;
        this.speed = speed;
        this.shooterTeam = team;
        this.canFriendlyFire = canFriendlyFire;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
       var target = null // todo add hp

        if (target != null)
        {
            if (target.PlayerTeam != shooterTeam || canFriendlyFire)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
