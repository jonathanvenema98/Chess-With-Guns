using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ExtensionMethods
{
    public static bool HasFlag(this Enum flags, Enum flag)
    {
        int flagsValue = (int) (object) flags;
        int flagValue = (int) (object) flag;
        return (flagsValue & flagValue) == flagValue;
    }
    
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

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action.Invoke(item);
        }

        return enumerable;
    }
}
