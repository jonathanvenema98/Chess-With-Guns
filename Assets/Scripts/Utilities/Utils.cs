using System;

public static class Utils
{
    public static bool HasFlag(this Enum flags, Enum flag)
    {
        int flagsValue = (int) (object) flags;
        int flagValue  = (int) (object) flag;
        return (flagsValue & flagValue) == flagValue;
    }

    #region Input

    public const int LeftMouseButton = 0;
    public const int RightMouseButton = 1;
    public const int MiddleMouseButton = 2;

    public const string VerticalAxis = "Vertical";
    public const string HorizontalAxis = "Horizontal";

    #endregion
}
