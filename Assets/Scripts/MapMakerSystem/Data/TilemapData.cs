using System.Collections.Generic;

[System.Serializable]
public class TilemapData
{
    public List<TileData> Tiles { get; set; }    
    
    public TilemapData() { }

    public TilemapData(List<TileData> tiles)
    {
        Tiles = tiles;
    }
}
