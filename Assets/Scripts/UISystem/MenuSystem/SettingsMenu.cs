using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider cameraSpeedSlider;
    public Text cameraSpeedValue;
    public Slider zoomSensitivitySlider;
    public Text zoomSensitivityValue;

    private void Start()
    {
        cameraSpeedSlider.value = PlayerPrefs.GetFloat("CameraSpeed", Settings.DefaultCameraSpeed);
        zoomSensitivitySlider.value = PlayerPrefs.GetFloat("ZoomSensitivity", Settings.DefaultZoomSensitivity);
    }

    public void SetCameraSpeed(float cameraSpeed)
    {
        PlayerPrefs.SetFloat("CameraSpeed", cameraSpeed);
    }
    public void SetCameraSpeedValue(float i)
    {
        cameraSpeedValue.text = cameraSpeedSlider.value.ToString("#.#");
    }
    public void SetZoomSensitivity(float zoomSensitivity)
    {
        PlayerPrefs.SetFloat("ZoomSensitivity", zoomSensitivity);
    }
    public void SetZoomSensitivityValue(float i)
    {
        zoomSensitivityValue.text = zoomSensitivitySlider.value.ToString("#.#");
    }
}
