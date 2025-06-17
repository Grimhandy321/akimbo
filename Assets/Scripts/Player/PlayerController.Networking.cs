using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public partial class PlayerController : Synchronizable
{
    public GameObject hat;

    public bool _isOwner = true;
    public bool _isHost = false;
    private bool _offline;
    private bool _possessed;

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
            _possessed = Avatar.IsPossessed;

            Avatar.OnPossessed.AddListener(_ =>
            {
                _isOwner = Avatar.IsOwner;
                _possessed = true;
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
        if (!_isOwner) return;

        if (playerTeam != team)
        {
            playerTeam = team;
            Commit(); 
            UpdateTeamVisuals(); 
        }
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        writer.Write((int)playerTeam);
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        Team syncedTeam = (Team)reader.ReadInt();

        if (playerTeam != syncedTeam)
        {
            playerTeam = syncedTeam;
            UpdateTeamVisuals(); 
        }
    }

    private void UpdateTeamVisuals()
    {
        if (hat == null) return;

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
