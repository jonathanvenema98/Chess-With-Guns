using System.Collections.Generic;
using UnityEngine;

public class Soldier : Piece
{
    private bool isFirstMove = true;

    public override IEnumerable<Vector2Int> GetMoves()
    {
        int moveSpaces = isFirstMove ? 2 : 1;
        
        return ActionBuilder.For(this)
            .Straight(moveSpaces, ActionOption.IsUnoccupied | ActionOption.ValidTerrainType | ActionOption.UntilBlocked)
            .Build();
    }

    public override IEnumerable<Vector2Int> GetAttacks()
    {
        return ActionBuilder.For(this)
            .Diagonal(1, ActionOption.IsEnemy | ActionOption.UntilBlocked)
            .Build();
    }

    public override void OnPieceMove()
    {
        Debug.Log("Piece has been moved!");
        isFirstMove = false;
    }
}
