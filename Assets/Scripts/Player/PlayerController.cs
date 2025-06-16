using Alteruna;
using System;
using UnityEngine;

public partial class PlayerController : Synchronizable
{


    void Start()
    {
        if (!Avatar.IsMe) return;
        InitializeMovement();
        InitializeWeapons();
        InitializeNetworking();
    }

    void Update()
    {
        if (_possesed && Avatar.IsMe)
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
        if (!Avatar.IsMe) return;
        MovementFixedUpdate(); 
    }

    private void OnPossession()
    {
        InitializeHealth();
        if (Avatar.IsMe)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void LateUpdate()
    {
        if (!Avatar.IsMe && !_offline)
        {
            transform.position = Vector3.Lerp(transform.position, _networkPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, _networkRotation, Time.deltaTime * 10);

            EquipWeapon(NetworkWeaponIndex); // volá se poøád
        }
    }
    private void OnDisable()
    {
        Commit();
        Sync();
    }


}
