using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    [SerializeField] private new Camera camera;

    [SerializeField] private float moveDuration;
    [SerializeField] private float mouseZoomSpeed;
    [SerializeField] private float keyZoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float focusedZoom;
    [SerializeField] private float unFocusThreshold;
    
    private float maxZoom;
    private Vector3 newPosition;
    private float newZoom;

    private bool isFocused;
    private Vector2Int focusedTile;
    
    // Start is called before the first frame update
    private void Start()
    {
        maxZoom = BoardController.BoardLength * 2 + 4;
        newPosition = transform.position;
        newZoom = camera.orthographicSize;
    }

    private void Update()
    {
        HandleMouseMovement();
        HandleKeyboardMovement();
        RestrictMovements();
        
        if (isFocused && newZoom - focusedZoom > unFocusThreshold)
            UnfocusCamera();
        
        ApplyMovement();
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

    private void HandleMouseMovement()
    {
        //Zooming
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += -Input.mouseScrollDelta.y * mouseZoomSpeed;
        }
    }

    private void HandleKeyboardMovement()
    {
        //Zoom
        if (Input.GetKey(KeyCode.Z))
        {
            newZoom -= keyZoomSpeed;
        }
        if (Input.GetKey(KeyCode.C))
        {
            newZoom += keyZoomSpeed;
        }
    }

    private void RestrictMovements()
    {
        //Restrict zoom
        if (newZoom < minZoom) newZoom = minZoom;
        else if (newZoom > maxZoom) newZoom = maxZoom;
    }

    private void ApplyMovement()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, moveDuration * Time.deltaTime);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, newZoom, moveDuration * Time.deltaTime);
    }
}
