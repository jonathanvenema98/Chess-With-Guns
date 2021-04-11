using UnityEngine;

public class MapEditorCameraController : AbstractCameraController<MapEditorCameraController>
{
    protected override void Initialise()
    {
    }

    protected override void UpdateFunctionality()
    {
        HandleZooming();
        HandleMovement();
    }

    public static void ResetPosition()
    {
        Instance.transform.position = Vector3.zero;
        Instance.newPosition = Vector3.zero;
    }
}
