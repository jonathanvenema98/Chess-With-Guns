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
    [SerializeField] private List<TerrainType> acceptableTerrainTypes;
    
    public Transform Transform => transform;
    public Team Team => team;
    public int MaxHealth => maxHealth;
    public AttackType AttackType => attackType;
    public int AttackDamage => attackDamage;
    public PieceType PieceType => pieceType;
    public List<TerrainType> AcceptableTerrainTypes => acceptableTerrainTypes;
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
    
    public virtual void OnDeath() { }
}

   











