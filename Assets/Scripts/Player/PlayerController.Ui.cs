using System;
using UnityEngine;
using Alteruna;

public partial class PlayerController : Synchronizable
{


    private void InitializeUI()
    {
        TeamSelectorUI.Instance?.SetController(this);
    }
}
