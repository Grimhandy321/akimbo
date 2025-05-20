using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Fire(Team team);

    public abstract void Detonate();
}

