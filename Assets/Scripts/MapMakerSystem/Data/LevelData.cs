using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelData
{
    public Vector3Int TilemapSize { get; set; }
    public Vector3Int TilemapOrigin { get; set; }
    
    public TilemapData TilemapData { get; set; }
    
    public LevelData() { }

    public LevelData(TilemapData tilemapData, Tilemap tilemap)
    {
        TilemapData = tilemapData;
        TilemapSize = tilemap.size;
        TilemapOrigin = tilemap.origin;
    }
}
