using System;
using UnityEngine;
using Alteruna;

public partial class PlayerController : Synchronizable
{


    private Gun[] weaponInstances;

    [SynchronizableField]
    private int activeWeaponIndex = 0;

    private Gun activeGun;

    private void InitializeWeapons()
    {
        if (weaponHolder == null || weaponHolder.childCount == 0)
        {
            Debug.LogWarning("no gun");
            return;
        }

        weaponInstances = new Gun[weaponHolder.childCount];

        for (int i = 0; i < weaponHolder.childCount; i++)
        {
            Transform child = weaponHolder.GetChild(i);
            Gun gun = child.GetComponent<Gun>();

            if (gun == null)
            {
                Debug.LogWarning($"{i} not Gun component.");
                continue;
            }

            weaponInstances[i] = gun;
            gun.controller = this;
        }

        SetActiveWeapon(activeWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        if (weaponInstances == null || index == activeWeaponIndex || index < 0 || index >= weaponInstances.Length)
            return;

        SetActiveWeapon(index);

        if (Avatar.IsMe)
        {
            activeWeaponIndex = index;
            Commit(); 
        }
    }

    private void SetActiveWeapon(int index)
    {
        for (int i = 0; i < weaponInstances.Length; i++)
        {
            if (weaponInstances[i] != null)
                weaponInstances[i].gameObject.SetActive(i == index);
        }

        activeWeaponIndex = index;
        activeGun = weaponInstances[activeWeaponIndex];
    }

    private void HandleWeaponSwitch()
    {
        if (weaponInstances == null || weaponInstances.Length == 0)
            return;
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

    private void WeaponUpdate()
    {
        if (!Avatar.IsMe) return; 
        HandleWeaponSwitch();
    }

    public Gun ActiveGun => activeGun;

    public void SyncWeaponIndex(int newIndex)
    {
        if (newIndex != activeWeaponIndex)
        {
            SetActiveWeapon(newIndex);
        }
    }

}
