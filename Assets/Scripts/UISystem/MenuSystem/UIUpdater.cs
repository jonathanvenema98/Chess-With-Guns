using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public TextMeshProUGUI status;

    private int remainingActions = 0;
    private Team team = 0;

    private void Start()
    {
        UpdateStatus();
        // zoomSensitivitySlider.value = PlayerPrefs.GetFloat("ZoomSensitivity", Settings.DefaultZoomSensitivity);
    }

    private void Update()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        status.text = remainingActions + " actions remaining. " + "Player's turn: " + team;
    }

    public void SetRemainingActions(int remainingActions)
    {
        this.remainingActions = remainingActions;
        if(team == 0)
        {
            this.team = Team.Blue;
        }
    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }
}
