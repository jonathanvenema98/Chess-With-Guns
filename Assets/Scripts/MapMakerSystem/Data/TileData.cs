using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public static readonly TileData ClearTilemap = new TileData(Vector3Int.zero, "ClearTilemap");
    
    public SerializableVector3Int CellPosition { get; set; }
    public string Name { get; set; }

    public TileData() { }
    
    public TileData(SerializableVector3Int cellPosition, string name)
    {
        CellPosition = cellPosition;
        Name = name;
    }

    public override bool Equals(object obj)
    {
        return obj is TileData tileData
               && tileData.Name == Name;
    }

    //Auto generated method
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        unchecked
        {
            return ((CellPosition != null ? CellPosition.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
        }
    }
}