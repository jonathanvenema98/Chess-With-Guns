[System.Serializable]
public class TileData
{
    public SerializableVector3Int TilePosition { get; set; }
    public string TileName { get; set; }

    public TileData() { }
    
    public TileData(SerializableVector3Int tilePosition, string tileName)
    {
        TilePosition = tilePosition;
        TileName = tileName;
    }
}