using UnityEngine;

[System.Serializable]
public class SerializableVector3Int
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public SerializableVector3Int() { }
    public SerializableVector3Int(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static implicit operator SerializableVector3Int(Vector3Int vector3Int)
        => new SerializableVector3Int(vector3Int.x, vector3Int.y, vector3Int.z);

    public static implicit operator Vector3Int(SerializableVector3Int vector3Int)
        => new Vector3Int(vector3Int.X, vector3Int.Y, vector3Int.Z);
}
