using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelData
{
    public Vector3Int TilemapSize { get; set; }
    public Vector3Int TilemapOrigin { get; set; }
    public TilemapData TilemapData { get; set; }
    public ListData<PieceData> PieceData { get; set; }
    
    public LevelData() { }

    public LevelData(TilemapData tilemapData, ListData<PieceData> pieceData, Tilemap tilemap)
    {
        TilemapData = tilemapData;
        PieceData = pieceData;
        TilemapSize = tilemap.size;
        TilemapOrigin = tilemap.origin;
    }
}
