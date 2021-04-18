using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Piece : MonoBehaviour, IBoardItem
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Team team;
    [SerializeField] private int maxHealth;
    [SerializeField] private AttackType attackType;
    [SerializeField] private int attackDamage;
    [SerializeField] private PieceType pieceType;
    [SerializeField] private List<TerrainType> acceptableTerrainTypes;
    
    public Transform Transform => transform;
    public Team Team => team;
    public int MaxHealth => maxHealth;
    public AttackType AttackType => attackType;
    public int AttackDamage => attackDamage;
    public PieceType PieceType => pieceType;
    public List<TerrainType> AcceptableTerrainTypes => acceptableTerrainTypes;
    public Sprite Sprite => sprite;
    public int CurrentHealth { get; protected set; }
    public bool IsDead { get; protected set; }
    public Vector2Int BoardPosition { get; set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public abstract IEnumerable<Vector2Int> GetMoves();

    public abstract IEnumerable<Vector2Int> GetAttacks();

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            CurrentHealth = 0;
            OnDeath();
        }
    }

    public virtual void OnPieceMove() { }

    public virtual void OnDeath()
    {
        BoardController.RemoveBoardItemAt(BoardPosition);
        Destroy(gameObject);
    }

    public Piece Instantiate(Transform parent)
    {
        Piece piece = Instantiate(this, parent);
        piece.name = name;
        return piece;
    }
    
    public override bool Equals(object other)
    {
        return other is Piece piece
               && piece.team == Team
               && piece.name == name;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

   











