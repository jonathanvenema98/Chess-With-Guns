using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Utils : Singleton<Utils>
{
    #region Input

    public const int LeftMouseButton = 0;
    public const int RightMouseButton = 1;
    public const int MiddleMouseButton = 2;

    public const string VerticalAxis = "Vertical";
    public const string HorizontalAxis = "Horizontal";

    public static Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
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
