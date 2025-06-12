using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public partial class PlayerController
{

    public GameObject hat;
    private bool _force;
    public bool _isOwner = true;
    public bool _isHost = false;
    private bool _offline;
    private bool _possesed;
    private Vector3 _networkPosition;
    private Quaternion _networkRotation;

    private void InitializeNetworking()
    {
        if (Avatar == null)
        {
            _isOwner = true;
            _isHost = true;
            _offline = true;
            OnPossession();
        }
        else
        {
            _isOwner = Avatar.IsOwner;
            _isHost = Avatar.Possessor.IsHost;
            _possesed = Avatar.IsPossessed;

            Avatar.OnPossessed.AddListener(_ =>
            {
                _isOwner = Avatar.IsOwner;
                _possesed = true;
                OnPossession();
            });

            if (Avatar.IsPossessed)
                OnPossession();
        }
    }

    private void Sync()
    {
        if (_isOwner && !_offline)
        {
            SyncUpdate();
        }
    }


    public void SetTeam(Team team)
    {
        playerTeam = team;
        Commit();
        UpdateTeamVisuals();
    }

    public override void Serialize(ITransportStreamWriter processor, byte LOD, bool forceSync = false)
    {
        _force = forceSync;
        base.Serialize(processor, LOD, forceSync);
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        writer.Write((int)playerTeam);
        writer.Write(transform.position);
        writer.Write(transform.rotation);
        writer.Write(activeWeaponIndex);
        UpdateTeamVisuals();
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        playerTeam = (Team)reader.ReadInt();
        _networkPosition = reader.ReadVector3();
        _networkRotation = reader.ReadQuaternion();
        _networkWeaponIndex = reader.ReadInt();
        UpdateTeamVisuals();
    }

    private void UpdateTeamVisuals()
    {
        Renderer rend = hat.GetComponent<Renderer>();
        if (rend == null) return;
        if (!rend.material.name.Contains("(Instance)"))
            rend.material = new Material(rend.material);

        switch (playerTeam)
        {
            case Team.Red:
                rend.material.color = Color.red;
                break;
            case Team.Blue:
                rend.material.color = Color.blue;
                break;
            case Team.Neutral:
                rend.material.color = Color.gray;
                break;
            default:
                rend.material.color = Color.white;
                break;
        }
    }
}
