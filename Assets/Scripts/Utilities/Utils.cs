using System;
using System.Collections;
using UnityEngine;

public class Utils : Singleton<Utils>
{
    public const int PixelsPerUnit = 16;
    public const float Pixel = 1F / PixelsPerUnit;
    
    private static Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    #region Input

    public const int LeftMouseButton = 0;
    public const int RightMouseButton = 1;
    public const int MiddleMouseButton = 2;

    public const string VerticalAxis = "Vertical";
    public const string HorizontalAxis = "Horizontal";

    public static Vector3 MouseWorldPosition => mainCamera.ScreenToWorldPoint(Input.mousePosition);
    
    #endregion

    #region Coroutines

    public static void After(float seconds, Action action)
    {
        IEnumerator Delay()
        {
            yield return new WaitForSecondsRealtime(seconds);
            action.Invoke();
        }

        Instance.StartCoroutine(Delay());
    }

    private void OnApplicationQuit()
    {
        Instance.StopAllCoroutines();
    }

    #endregion

}
