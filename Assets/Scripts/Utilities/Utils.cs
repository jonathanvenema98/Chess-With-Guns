using System;

public static class Utils
{
    public static bool HasFlag(this Enum flags, Enum flag)
    {
        int flagsValue = (int) (object) flags;
        int flagValue  = (int) (object) flag;
        return (flagsValue & flagValue) == flagValue;
    }
}
