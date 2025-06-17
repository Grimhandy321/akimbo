using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Gun",menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string name;
    public float fireRate;
    public AudioClip shootsound;
    public GameObject projectile;
}
