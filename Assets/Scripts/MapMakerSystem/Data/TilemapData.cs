using System.Collections.Generic;

[System.Serializable]
public class TilemapData
{
    public List<TileData> ChangedTiles { get; set; }    
    
    public TilemapData() { }

    public TilemapData(List<TileData> changedTiles)
    {
        ChangedTiles = changedTiles;
    }
}
