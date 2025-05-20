using System;
using UnityEngine;
public partial class PlayerController
{
    [Header("Weapon Switching")]
    public Gun[] weapons;
    private int activeWeaponIndex = 0;
    private Gun activeGun;
    private void WeaponUpdate() 
    {
        HandleWeaponSwitch();
        }
    private void InitializeWeapons() 
        {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == activeWeaponIndex);
            weapons[i].controller = this;
        }

        activeGun = weapons[activeWeaponIndex];
    }

    private void EquipWeapon(int index)
    {
        if (index == activeWeaponIndex || index < 0 || index >= weapons.Length)
            return;
        if (activeGun != null)
            activeGun.gameObject.SetActive(false);

        activeWeaponIndex = index;
        activeGun = weapons[activeWeaponIndex];
        activeGun.gameObject.SetActive(true);
        activeGun.controller = this;
    }
    private void HandleWeaponSwitch()
    {

        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
                break;
    }
}

        if (Input.mouseScrollDelta.y > 0)
            EquipWeapon((activeWeaponIndex + 1) % weapons.Length);
        else if (Input.mouseScrollDelta.y < 0)
            EquipWeapon((activeWeaponIndex - 1 + weapons.Length) % weapons.Length);
    }
}
