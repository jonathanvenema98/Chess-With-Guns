using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour, IBoardItem
{
    public Vector2Int BoardPosition { get; set; }
}
