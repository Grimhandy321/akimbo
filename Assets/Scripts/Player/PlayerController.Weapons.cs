using System;
using UnityEngine;
using Alteruna;

public partial class PlayerController : Synchronizable
{
    [Header("Weapon Switching")]
    public Gun[] weaponPrefabs;
    private Gun[] weaponInstances;

    [SynchronizableField]
    private int _networkWeaponIndex = 0;

    private int activeWeaponIndex = 0;
    private Gun activeGun;


    private void InitializeWeapons()
    {
        if (weaponPrefabs == null || weaponPrefabs.Length == 0)
        {
            Debug.LogWarning("No weapon prefabs assigned!");
            return;
        }

        weaponInstances = new Gun[weaponPrefabs.Length];

        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            weaponInstances[i] = Instantiate(weaponPrefabs[i], weaponHolder);
            weaponInstances[i].transform.localPosition = Vector3.zero;
            weaponInstances[i].transform.localRotation = Quaternion.identity;

            weaponInstances[i].gameObject.SetActive(i == activeWeaponIndex);
            weaponInstances[i].controller = this;

            if (Camera.main != null)
                weaponInstances[i].fireOrigin = Camera.main.transform;
            else
                Debug.LogWarning("Camera.main not found! FireOrigin not set.");
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

        // Synchronizace sítě
        _networkWeaponIndex = activeWeaponIndex;
        // Zde můžeš přidat síťový sync, např. RPC volání, pokud používáš Alteruna k synchronizaci zbraní.
    }

    private void HandleWeaponSwitch()
    {
        if (weaponInstances == null || weaponInstances.Length == 0) return;
..
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
        HandleWeaponSwitch();
    }

    public Gun ActiveGun => activeGun;
    public void SyncWeaponIndex(int newIndex)
    {
        if (newIndex != activeWeaponIndex)
        {
            EquipWeapon(newIndex);
        }
    }
}
