using AlterunaFPS;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelectorUI : MonoBehaviour
{
    public GameObject uiPanel;
    public Button redButton;
    public Button blueButton;

    public static TeamSelectorUI Instance { get; private set; }

    private PlayerController _controller;

    private void Awake()
    {
        Instance = this;
        uiPanel.SetActive(true);
    }

    void Start()
    {
        uiPanel.SetActive(false);
        redButton.onClick.AddListener(() => ChooseTeam(Team.Red));
        blueButton.onClick.AddListener(() => ChooseTeam(Team.Blue));
    }

    private void Update()
    {
        if (_controller == null || !_controller._isOwner)
            return;

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

    public void SetController(PlayerController controller)
    {
        _controller = controller;
    }

    private void ChooseTeam(Team team)
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _controller?.SetTeam(team);
    }
}