using Alteruna;
using System;
using UnityEngine;

public partial class PlayerController : Synchronizable
{


    void Start()
    {
        if (!Avatar.IsMe) return;
        InitializeMovement();
        InitializeNetworking();
        InitializeWeapons();
        InitializeHealth();
    }

    void Update()
    {
        if (_possessed && Avatar.IsMe)
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
        if (Avatar.IsMe)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void LateUpdate()
    {

    }
    private void OnDisable()
    {
        Commit();
        Sync();
    }


}
