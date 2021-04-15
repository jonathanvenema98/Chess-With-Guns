using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils : Singleton<Utils>
{
    public const int PixelsPerUnit = 16;
    public const float Pixel = 1F / PixelsPerUnit;
    public const double FloatingPointTolerance = 1e-5D;
    
    private static Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    #region Math

    public static bool NotEqual(float a, float b)
    {
        return Math.Abs(a - b) > FloatingPointTolerance;
    }

    public static bool Equal(float a, float b)
    {
        return Math.Abs(a - b) < FloatingPointTolerance;
    }

    #endregion
    
    #region Input

    public const int LeftMouseButton = 0;
    public const int RightMouseButton = 1;
    public const int MiddleMouseButton = 2;

    public const string VerticalAxis = "Vertical";
    public const string HorizontalAxis = "Horizontal";

    public static Vector3 MouseWorldPosition => mainCamera.ScreenToWorldPoint(Input.mousePosition);
    
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    
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
