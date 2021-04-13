using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider cameraSpeedSlider;
    public Slider zoomSensitivitySlider;

    private void Start()
    {
        cameraSpeedSlider.value = PlayerPrefs.GetFloat("CameraSpeed", Settings.DefaultCameraSpeed);
        zoomSensitivitySlider.value = PlayerPrefs.GetFloat("ZoomSensitivity", Settings.DefaultZoomSensitivity);
    }

    public void SetCameraSpeed(float cameraSpeed)
    {
        PlayerPrefs.SetFloat("CameraSpeed", cameraSpeed);
    }
    public void SetZoomSensitivity(float zoomSensitivity)
    {
        PlayerPrefs.SetFloat("ZoomSensitivity", zoomSensitivity);
        Debug.Log(PlayerPrefs.GetFloat("ZoomSensitivity"));
    }
}
