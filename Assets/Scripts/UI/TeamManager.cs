using Alteruna;
using System.Collections.Generic;
using UnityEngine;
using Alteruna.Trinity;

public class TeamManager : Synchronizable
{
    public static TeamManager Instance;

    private Dictionary<ushort, Team> _teams = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AssignTeam(ushort userID, Team team)
    {
        _teams[userID] = team;
        Multiplayer.Sync(this, Reliability.Reliable);
    }

    public Team GetTeam(ushort userID)
    {
        return _teams.TryGetValue(userID, out var team) ? team : Team.None;
    }

    public List<ushort> GetTeamPlayers(Team team)
    {
        var list = new List<ushort>();
        foreach (var kvp in _teams)
            if (kvp.Value == team)
                list.Add(kvp.Key);
        return list;
    }

    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(_teams.Count);
        foreach (var kvp in _teams)
        {
            writer.Write(kvp.Key);
            writer.Write((int)kvp.Value);
        }
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        _teams.Clear();
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            ushort id = reader.ReadUshort();
            Team team = (Team)reader.ReadInt();
            _teams[id] = team;
        }
    }
}
