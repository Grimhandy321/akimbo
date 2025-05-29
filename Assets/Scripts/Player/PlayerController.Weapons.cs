using System;
using UnityEngine;

public partial class PlayerController 
{
    [Header("Weapon Switching")]
    public Gun[] weaponPrefabs; // Prefabs in Inspector
    private Gun[] weaponInstances; // Instantiated at runtime

    private int activeWeaponIndex = 0;
    private Gun activeGun;
    private void WeaponUpdate()
    {
        HandleWeaponSwitch();
    }

    private void InitializeWeapons()
    {
        weaponInstances = new Gun[weaponPrefabs.Length];

        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            weaponInstances[i] = Instantiate(weaponPrefabs[i], weaponHolder);
            weaponInstances[i].gameObject.SetActive(i == activeWeaponIndex);
            weaponInstances[i].controller = this;
        }

        activeGun = weaponInstances[activeWeaponIndex];
    }

    private void EquipWeapon(int index)
    {
        if (index == activeWeaponIndex || index < 0 || index >= weaponInstances.Length)
            return;

        if (activeGun != null)
            activeGun.gameObject.SetActive(false);

        activeWeaponIndex = index;
        activeGun = weaponInstances[activeWeaponIndex];
        activeGun.gameObject.SetActive(true);
        activeGun.controller = this;
    }

    private void HandleWeaponSwitch()
    {
        for (int i = 0; i < weaponInstances.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
                return;
            }
        }

        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0)
        {
            EquipWeapon((activeWeaponIndex + 1) % weaponInstances.Length);
        }
        else if (scroll < 0)
        {
            EquipWeapon((activeWeaponIndex - 1 + weaponInstances.Length) % weaponInstances.Length);
        }
    }

    public Gun ActiveGun => activeGun;
}

