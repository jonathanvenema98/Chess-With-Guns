using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ExtensionMethods
{
    #region Enum Flags

    public static bool HasFlag<T>(this T flagEnum, T flag) where T : IConvertible
    {
        int flagEnumValue = (int) (IConvertible) flagEnum;
        int flagValue     = (int) (IConvertible) flag;
        return (flagEnumValue & flagValue) == flagValue;
    }

    public static IEnumerable<T> GetFlags<T>(this T flagEnum) where T : IConvertible
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(flag => (int) (IConvertible) flag != 0 && HasFlag(flagEnum, flag));
    }

    #endregion

    public static List<Vector3Int> GetAllTilePositions(this Tilemap tilemap)
    {
        var tilePositions = new List<Vector3Int>();
        
        tilemap.CompressBounds();
        Vector3Int bottomLeftCorner = tilemap.origin;
        
        for (int x = 0; x < tilemap.size.x; x++)
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                Vector3Int tilePositon = new Vector3Int(x + bottomLeftCorner.x, y + bottomLeftCorner.y, 0);
                if (tilemap.HasTile(tilePositon))
                {
                    tilePositions.Add(tilePositon);
                }
            }
        }

        return tilePositions;
    }

    public static bool IsEmpty(this Tilemap tilemap)
    {
        tilemap.CompressBounds();
        //The tilemap size will always be 1 on the z-axis
        return tilemap.size == Vector3Int.forward;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action.Invoke(item);
        }

        return enumerable;
    }

    public static void AddAll<TV, TK>(this Dictionary<TV, TK> dictionary, IEnumerable<KeyValuePair<TV, TK>> pairs)
    {
        foreach (var pair in pairs)
        {
            dictionary.Add(pair.Key, pair.Value);
        }
    }
    
    public static T GetModule<T>(this GameObject gameObject, int index = 0) where T : Module
    {
        return gameObject.GetComponentsInChildren<T>()[index];
    }
}
