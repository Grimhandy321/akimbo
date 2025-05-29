using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Alteruna;

public class TeamPickerUI : MonoBehaviour
{
    public GameObject panel;
    public Button joinRedButton;
    public Button joinBlueButton;
    public Transform redList;
    public Transform blueList;
    public GameObject playerEntryPrefab;

    private void Start()
    {
        panel.SetActive(false);

        Multiplayer.Instance.OnRoomJoined.AddListener((_, _, user) =>
        {
            if (user.Index == Multiplayer.Instance.Me.Index)
                panel.SetActive(true);
        });

        joinRedButton.onClick.AddListener(() => JoinTeam(Team.Red));
        joinBlueButton.onClick.AddListener(() => JoinTeam(Team.Blue));
    }

    void JoinTeam(Team team)
    {
        ushort userId = Multiplayer.Instance.Me.Index;
        TeamManager.Instance.AssignTeam(userId, team);
        panel.SetActive(false);
    }

    void Update()
    {
        UpdateLists();
    }

    void UpdateLists()
    {
        foreach (Transform child in redList) Destroy(child.gameObject);
        foreach (Transform child in blueList) Destroy(child.gameObject);

        foreach (var user in Multiplayer.Instance.GetUsers())
        {
            if (user == null) continue;

            var team = TeamManager.Instance.GetTeam(user.Index);
            if (team == Team.None) continue;

            var entry = Instantiate(playerEntryPrefab);
            entry.GetComponentInChildren<TMP_Text>().text = user.Name;
            entry.transform.SetParent(team == Team.Red ? redList : blueList, false);
        }
    }
}
