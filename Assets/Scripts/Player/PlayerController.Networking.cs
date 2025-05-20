using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public partial class PlayerController
{
    private Vector2 _lastPosition;
    private bool _force;
    private bool _isOwner = true;
    private bool _isHost = false;
    private bool _offline;
    private bool _possesed;

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

    public override void Serialize(ITransportStreamWriter processor, byte LOD, bool forceSync = false)
    {
        _force = forceSync;
        base.Serialize(processor, LOD, forceSync);
    }

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        Vector3 p = transform.position;
        byte flags = _force
            ? (byte)255
            : (byte)(
                (Mathf.Abs(_lastPosition.x - p.x) + Mathf.Abs(_lastPosition.y - p.y) > 0.1f ? 4 : 0) |
                (gameObject.activeSelf ? 16 | 4 : 0)
            );

        writer.Write(flags);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        byte flags = reader.ReadByte();
        gameObject.SetActive((flags & 16) != 0);
    }
}
