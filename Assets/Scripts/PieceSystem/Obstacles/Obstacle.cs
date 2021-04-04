using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour, IBoardItem
{
    public Transform Transform
    {
        get
        {
            return transform;
        }
    }

    public Vector2Int BoardPosition { get; set; }
}
