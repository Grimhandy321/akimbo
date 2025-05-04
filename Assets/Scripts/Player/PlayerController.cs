using Alteruna;
using System;
using UnityEngine;

public partial class PlayerController : Synchronizable
{


    void Start()
    {
        InitializeMovement(); 
        InitializeGrapple();  
    }

    void Update()
    {
        HandleShooting();
        MovementUpdate();     
        GrappleUpdate();    
    }

    void FixedUpdate()
    {
        MovementFixedUpdate(); 
    }



   

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(currentHealth);
        writer.Write((int)playerTeam);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        currentHealth = reader.ReadFloat();
        playerTeam = (Team)reader.ReadInt();
    }
}
