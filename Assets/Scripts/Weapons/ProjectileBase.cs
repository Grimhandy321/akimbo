using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public Team Team { get; protected set; }
    public ushort SenderId { get; protected set; }
    protected bool hasDetonated = false;

    public abstract void Fire(Vector3 position, Vector3 direction, Team team, ushort senderId);

    public virtual void Detonate()
    {
        if (hasDetonated) return;
        hasDetonated = true;
        Destroy(gameObject);
    }
}