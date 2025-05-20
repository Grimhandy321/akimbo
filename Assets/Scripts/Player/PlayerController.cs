using Alteruna;
using System;
using UnityEngine;

public partial class PlayerController : Synchronizable
{


    void Start()
    {
        if (!_isOwner) return;
        InitializeMovement();
        InitializeNetworking();
    }

    void Update()
    {
        if (_possesed && _isOwner)
        {
            WeaponUpdate();
            HandleShooting();
            MovementUpdate();
            GrappleUpdate();
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

    private void OnDisable()
    {
        Commit();
        Sync();
    }


}
