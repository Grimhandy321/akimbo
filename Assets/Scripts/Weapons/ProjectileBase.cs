using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Fire(Vector3 position, Vector3 direction, Team team,ushort senderId);
    public abstract void Detonate();
}
