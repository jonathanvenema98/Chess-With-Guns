using UnityEngine;

[System.Serializable]
public class PieceData
{
    public string Name { get; set; }
    public Team Team { get; set; }
    public Vector3Int CellPosition { get; set; }
}
