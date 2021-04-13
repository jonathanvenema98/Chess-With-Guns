using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider zoomSpeedSlider;
    public Slider zoomSensitivitySlider;

    private void Start()
    {
        zoomSpeedSlider.value = PlayerPrefs.GetFloat("ZoomSpeed", DefaultOptions.DefaultZoomSpeed);
        zoomSensitivitySlider.value = PlayerPrefs.GetFloat("ZoomSensitivity", DefaultOptions.DefaultZoomSensitivity);
    }

    public void SetZoomSpeed(float zoomSpeed)
    {
        PlayerPrefs.SetFloat("ZoomSpeed", zoomSpeed);
    }
    public void SetZoomSensitivity(float zoomSensitivity)
    {
        PlayerPrefs.SetFloat("ZoomSensitivity", zoomSensitivity);
        Debug.Log(PlayerPrefs.GetFloat("ZoomSensitivity"));
    }
}
