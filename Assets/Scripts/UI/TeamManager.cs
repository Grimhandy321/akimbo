using Alteruna;
using System.Collections.Generic;
using UnityEngine;
using Alteruna.Trinity;

public class TeamManager : MonoBehaviour
{
    public Multiplayer multiplayer;
    public GameObject teamPickerUI;

    private Team selectedTeam = Team.None;

    void Start()
    {
        teamPickerUI.SetActive(false);
    }

    void OnConnected(User user)
    {
        teamPickerUI.SetActive(true);
    }

    public void ChooseRedTeam()
    {
        selectedTeam = Team.Red;
        teamPickerUI.SetActive(false);
        SpawnAvatarAtTeamPoint();
    }

    public void ChooseBlueTeam()
    {
        selectedTeam = Team.Blue;
        teamPickerUI.SetActive(false);
        SpawnAvatarAtTeamPoint();
    }

    void SpawnAvatarAtTeamPoint()
    {
        int spawnIndex = -1;

        if (selectedTeam == Team.Red)
        {
 
            spawnIndex = Random.Range(0, 2);
        }
        else if (selectedTeam == Team.Blue)
        {
            spawnIndex = Random.Range(2, 4);
        }

        if (spawnIndex >= 0)
        {
            multiplayer.SpawnAvatar(multiplayer.AvatarSpawnLocations[spawnIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid spawn index or team not selected.");
        }
    }
}

