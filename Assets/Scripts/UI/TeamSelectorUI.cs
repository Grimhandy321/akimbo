using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectorUI : MonoBehaviour
{
    public GameObject uiPanel;
    public Button redButton;
    public Button blueButton;
    public PlayerController controller;

    void Start()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        redButton.onClick.AddListener(() => ChooseTeam(Team.Red));
        blueButton.onClick.AddListener(() => ChooseTeam(Team.Blue));
    }
    private void Update()
    {
        if (controller._isOwner)
        {

            if (Input.GetKeyDown(KeyCode.T))
            {
                 uiPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetKeyUp(KeyCode.T))
            {
                uiPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void ChooseTeam(Team team)
    {
        uiPanel.SetActive(false);
        controller.SetTeam(team);
    }
}

