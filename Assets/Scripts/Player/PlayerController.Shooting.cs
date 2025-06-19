using System;
using UnityEngine;
using Alteruna;

public partial class PlayerController 
{
    [SynchronizableField]
    public Team playerTeam = Team.Neutral;
    public Transform weaponHolder;
    public void HandleShooting()
    {
        if (!Avatar.IsMe || activeGun == null)
            return;

        if (Input.GetMouseButton(0))
        {
            activeGun.Shoot(multiplayer.Me.Index);
        }
    }
}
