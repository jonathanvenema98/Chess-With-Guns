using System.Collections.Generic;
using UnityEngine;

public class Tank : Piece
{
    public override IEnumerable<Vector2Int> GetMoves()
    {
        return ActionBuilder.For(this)
            .AnyDirection(2, ActionOption.IsUnoccupied | ActionOption.ValidTerrainType | ActionOption.UntilBlocked)
            .Build();
    }

    public override IEnumerable<Vector2Int> GetAttacks()
    {
        return ActionBuilder.For(this)
            .Diagonal(2, ActionOption.IsEnemy | ActionOption.UntilBlocked)
            .Build();
    }
}
