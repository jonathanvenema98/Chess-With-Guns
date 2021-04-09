[System.Serializable]
public class LevelData
{
    public TilemapData TilemapData { get; set; }
    
    public LevelData() { }

    public LevelData(TilemapData tilemapData)
    {
        TilemapData = tilemapData;
    }
}
