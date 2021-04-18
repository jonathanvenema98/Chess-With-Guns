using System.Collections.Generic;
using UnityEngine;

public class General : Piece
{
    public override IEnumerable<Vector2Int> GetMoves()
    {
        return ActionBuilder.For(this)
            .AnyDirection(1, ActionOption.IsUnoccupied | ActionOption.ValidTerrainType | ActionOption.UntilBlocked)
            .Build();
    }

    public override IEnumerable<Vector2Int> GetAttacks()
    {
        return ActionBuilder.For(this)
            .AnyDirection(1, ActionOption.IsEnemy | ActionOption.UntilBlocked)
            .Build();
    }

    public override void OnDeath()
    {
        StateMachine.Instance.SetState(new GameOverState());
        base.OnDeath();
    }
}
