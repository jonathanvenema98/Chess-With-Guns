using UnityEngine;

public interface IBoardItem
{
    Transform Transform { get; }
    Vector2Int BoardPosition { get; set; }
}
