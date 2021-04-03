using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Obstacle : MonoBehaviour, IBoardItem
{
    public Vector2Int BoardPosition { get; set; }
}
