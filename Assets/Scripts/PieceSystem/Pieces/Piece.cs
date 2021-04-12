using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Piece : MonoBehaviour, IBoardItem
{
    [SerializeField] private Team team;
    [SerializeField] private int maxHealth;
    [SerializeField] private AttackType attackType;
    [SerializeField] private int attackDamage;
    [SerializeField] private PieceType pieceType;
    
    public Transform Transform => transform;
    public Vector2Int BoardPosition { get; set; }
    public Team Team { get; private set; }
    public int MaxHealth { get; private set; }
    public AttackType AttackType { get; private set; }
    public int AttackDamage { get; private set; }
    public PieceType PieceType { get; private set; }
    public int CurrentHealth { get; protected set; }
    public bool IsDead { get; protected set; }

    private void Start()
    {
        Team = team;
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        AttackType = attackType;
        AttackDamage = attackDamage;
        PieceType = pieceType;
    }
    
    
    public abstract IEnumerable<Vector2Int> GetRelativeMoves();

    public abstract IEnumerable<Vector2Int> ValidateRelativeMoves(IEnumerable<Vector2Int> relativeMoves);

    public IEnumerable<Vector2Int> GetMoves()
    {
        return ValidateRelativeMoves(
            ConvertRelativePositions(
                GetRelativeMoves()));
    }
    
    public IEnumerable<Vector2Int> GetAttacks()
    {
        return ValidateRelativeAttacks(
            ConvertRelativePositions(
                GetRelativeAttacks()));
    }

    public abstract IEnumerable<Vector2Int> GetRelativeAttacks();

    public abstract IEnumerable<Vector2Int> ValidateRelativeAttacks(IEnumerable<Vector2Int> relativeAttacks);
    
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            CurrentHealth = 0;
        }
    }

    //Changes the relative positions to be based on the particular team that the piece is on.
    public IEnumerable<Vector2Int> ConvertRelativePositions(IEnumerable<Vector2Int> relativePositions)
    {
        return relativePositions.Select(position => Team == Team.Blue ? position : -position);
    }

    public virtual void OnPieceMove() { } 
}

   











