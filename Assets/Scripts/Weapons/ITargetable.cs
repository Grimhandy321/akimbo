using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable 
{
    public void Damage(Team team, float dmg,ushort senderId);
    public void Heal(float hp);
}
