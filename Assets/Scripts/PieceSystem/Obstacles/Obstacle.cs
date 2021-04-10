using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour, IBoardItem
{
    public Transform Transform => transform;

    public Vector2Int BoardPosition { get; set; }
    
}
