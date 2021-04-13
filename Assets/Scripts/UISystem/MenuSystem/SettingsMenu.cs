using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider zoomSpeedSlider;
    public Slider zoomSensitivitySlider;

    void Start()
    {
        zoomSpeedSlider.value = PlayerPrefs.GetFloat("ZoomSpeed");
        zoomSensitivitySlider.value = PlayerPrefs.GetFloat("ZoomSensitivity");
    }

    public void SetZoomSpeed (float zoomSpeed)
    {
        PlayerPrefs.SetFloat("ZoomSpeed", zoomSpeed);
    }
    public void SetZoomSensitivity (float zoomSensitivity)
    {
        PlayerPrefs.SetFloat("ZoomSensitivity", zoomSensitivity);
        Debug.Log(PlayerPrefs.GetFloat("ZoomSensitivity"));
    }
}
