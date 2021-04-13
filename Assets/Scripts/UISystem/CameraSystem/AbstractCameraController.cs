using UnityEngine;

public abstract class AbstractCameraController<T> : Singleton<T> where T: Singleton<T>
{
    [Header("General")]
    [SerializeField] protected new Camera camera;
    [SerializeField] protected float cameraSpeed;

    [Header("Zooming")]
    [SerializeField] protected float initialZoomIn;
    [SerializeField] protected float mouseZoomSensitivity;
    [SerializeField] protected float keyZoomSpeed;
    [SerializeField] protected float minZoom;

    [Header("Movement")]
    [SerializeField] protected float normalKeyMoveSensitivity;
    [SerializeField] protected float fastKeyMoveSensitivity;
    [SerializeField] protected bool usePanning;
    [SerializeField] protected float panMoveSpeed;
    [Tooltip("Note: This is in pixels"), SerializeField] protected Vector2 panBorder;

    protected float maxZoom = -1;
    protected Vector3 newPosition;
    protected float newZoom;
    protected float keyMoveSpeed;
    private bool hasMaxZoom;

    // Start is called before the first frame update
    private void Start()
    {
        Initialise();

        cameraSpeed = PlayerPrefs.GetFloat("CameraSpeed", Settings.DefaultCameraSpeed);
        mouseZoomSensitivity = PlayerPrefs.GetFloat("ZoomSensitivity", Settings.DefaultZoomSensitivity);

        hasMaxZoom = Utils.NotEqual(maxZoom, -1);

        newPosition = transform.position;

        newZoom = hasMaxZoom ? maxZoom : camera.orthographicSize;
        camera.orthographicSize = (hasMaxZoom ? maxZoom : minZoom) + initialZoomIn;
    }

    protected abstract void Initialise();

    protected abstract void UpdateFunctionality();

    protected void ResetZoom()
    {
        newZoom = hasMaxZoom ? maxZoom : camera.orthographicSize;
    }
    
    protected void Update()
    {
        keyMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? fastKeyMoveSensitivity : normalKeyMoveSensitivity;

        if (UIController.IsUIActive)
            return;
        
        UpdateFunctionality();
        RestrictZoom();
        RestrictMovement();
        
        ApplyMovement();
    }

    protected void HandleZooming()
    {
        //Zooming
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += -Input.mouseScrollDelta.y * mouseZoomSensitivity;
        }
        
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

    protected void HandleMovement()
    {
        //Panning
        if (usePanning && Input.mousePosition.x >= 0 && Input.mousePosition.x < Screen.width
            && Input.mousePosition.y >= 0 && Input.mousePosition.y < Screen.height)
        {
            
            if (Input.mousePosition.y > Screen.height - panBorder.y)
            {
                newPosition += transform.up * panMoveSpeed;
            }

            if (Input.mousePosition.y < panBorder.y)
            {
                newPosition -= transform.up * panMoveSpeed;
            }

            if (Input.mousePosition.x > Screen.width - panBorder.x)
            {
                newPosition += transform.right * panMoveSpeed;
            }

            if (Input.mousePosition.x < panBorder.x)
            {
                newPosition -= transform.right * panMoveSpeed;
            }
        }

        //Movement
        if (Input.GetAxis(Utils.VerticalAxis) != 0)
        {
            newPosition += transform.up * (Input.GetAxis(Utils.VerticalAxis) * keyMoveSpeed);
        }
        if (Input.GetAxis(Utils.HorizontalAxis) != 0)
        {
            newPosition += transform.right * (Input.GetAxis(Utils.HorizontalAxis) * keyMoveSpeed);
        }
    }

    protected virtual void RestrictZoom()
    {
        //Restrict zoom
        if (newZoom < minZoom) newZoom = minZoom;
        else if (hasMaxZoom && newZoom > maxZoom) newZoom = maxZoom;
    }
    
    protected virtual void RestrictMovement() {}

    private void ApplyMovement()
    {
        if (newPosition != transform.position)
            transform.position = Vector3.Lerp(transform.position, newPosition, cameraSpeed * Time.deltaTime);
        if (Utils.NotEqual(newZoom, camera.orthographicSize))
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, newZoom, cameraSpeed * Time.deltaTime);
    }
}
