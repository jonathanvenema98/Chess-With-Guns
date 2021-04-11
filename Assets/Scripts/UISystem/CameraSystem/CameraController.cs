using UnityEngine;

public class CameraController : AbstractCameraController<CameraController>
{
    [Header("Focus Zooming")]
    [SerializeField] private float focusedZoom;
    [SerializeField] private float unFocusThreshold;

    private bool isFocused;
    private Vector2Int focusedTile;

    protected override void Initialise()
    {
        maxZoom = BoardController.BoardLength + 4;
    }

    protected override void UpdateFunctionality()
    {
        HandleZooming();
        HandleMovement();
        
        if (isFocused && newZoom - focusedZoom > unFocusThreshold)
            UnfocusCamera();
    }

    public void OnTileLeftClickedSubscriber(Vector2Int boardPosition)
    {
        if (isFocused && boardPosition == focusedTile)
        {
            UnfocusCamera();
        }
        else
        {
            FocusCamera(boardPosition);
        }
    }

    public void UnfocusCamera()
    {
        isFocused = false;
        BoardController.DestroyBorderAt(focusedTile);
        
        newPosition = new Vector3(0, 0, newPosition.z);
        newZoom = maxZoom;
    }
    
    public void FocusCamera(Vector2Int boardPosition)
    {
        isFocused = true;
        BoardController.DestroyBorderAt(focusedTile);
        focusedTile = boardPosition;
        BoardController.ShowBorderAt(focusedTile, GameController.FocusedTileColour);
        
        newPosition = BoardController.BoardPositionToWorldPosition(boardPosition);
        newZoom = focusedZoom;
    }
}
