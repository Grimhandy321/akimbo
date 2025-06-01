using System;
using UnityEngine;

public partial class PlayerController
{
    public Team playerTeam = Team.Neutral;
    public Transform weaponHolder;
    public static Action shootInput;
    public void HandleShooting()
    {
        weaponInstances[activeWeaponIndex].fireOrigin = cam.transform;
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
    }
}
