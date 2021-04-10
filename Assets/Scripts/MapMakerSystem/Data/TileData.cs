using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public static TileData ClearTilemap = new TileData(Vector3Int.zero, "ClearTilemap");
    
    public SerializableVector3Int TilePosition { get; set; }
    public string TileName { get; set; }

    public TileData() { }
    
    public TileData(SerializableVector3Int tilePosition, string tileName)
    {
        TilePosition = tilePosition;
        TileName = tileName;
    }

    public override bool Equals(object obj)
    {
        return obj is TileData tileData
               && tileData.TileName == TileName;
    }

    //Auto generated method
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        unchecked
        {
            return ((TilePosition != null ? TilePosition.GetHashCode() : 0) * 397) ^ (TileName != null ? TileName.GetHashCode() : 0);
        }
    }
}