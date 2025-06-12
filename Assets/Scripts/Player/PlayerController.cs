using Alteruna;
using System;
using UnityEngine;

public partial class PlayerController : Synchronizable
{


    void Start()
    {
        if (!_isOwner) return;
        InitializeMovement();
        InitializeWeapons();
        InitializeNetworking();
    }

    void Update()
    {
        if (_possesed && _isOwner)
        {
            UpdateCoinThrowing();
            WeaponUpdate();
            HandleShooting();
            MovementUpdate();
            GrappleUpdate();
            HandleMouseLook();
        }
    }

    void FixedUpdate()
    {
        if (!_isOwner) return;
        MovementFixedUpdate(); 
    }

    private void OnPossession()
    {
        InitializeHealth();
        if (_isOwner)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void LateUpdate()
    {
        if (!_isOwner && !_offline)
        {
            transform.position = Vector3.Lerp(transform.position, _networkPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, _networkRotation, Time.deltaTime * 10);

            if (_networkWeaponIndex != activeWeaponIndex)
            {
                EquipWeapon(_networkWeaponIndex);
            }
        }
    }
    private void OnDisable()
    {
        Commit();
        Sync();
    }


}
