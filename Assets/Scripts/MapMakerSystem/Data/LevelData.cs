using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector3Int TilemapCentre { get; set; }

    public TilemapData TilemapData { get; set; }
    
    public LevelData() { }

    public LevelData(TilemapData tilemapData)
    {
        TilemapData = tilemapData;
    }
}
