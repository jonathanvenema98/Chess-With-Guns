using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionBuilder
{
	private readonly Piece piece;
	private readonly Team team;
	
	private readonly List<Vector2Int> actions;

	private Vector2Int currentAction;
	private bool isBlocked;

	private static readonly Dictionary<ActionOption, Func<ActionBuilder, bool>> ActionValidations
		= new Dictionary<ActionOption, Func<ActionBuilder, bool>>
		{
			{ActionOption.IsEnemy,      builder => BoardController.IsEnemyAt(builder.currentAction, builder.team)},
			{ActionOption.IsFriendly,   builder => BoardController.IsFriendlyAt(builder.currentAction, builder.team)},
			{ActionOption.IsObstacle,   builder => BoardController.IsObstacleAt(builder.currentAction)},
			{ActionOption.IsUnoccupied, builder => !BoardController.IsBoardItemAt(builder.currentAction)},
			{ActionOption.ValidTerrainType, builder => BoardController.CanPieceMoveTo(builder.piece, builder.currentAction)},
			{ActionOption.UntilBlocked, builder => !builder.isBlocked}
		};

	private static readonly List<Vector2Int> StraightDirections = new List<Vector2Int>
	{
		Vector2Int.up, Vector2Int.left, Vector2Int.right, Vector2Int.down
	};

	private static readonly List<Vector2Int> DiagonalDirections = new List<Vector2Int>
	{
		new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1)
	};

	private ActionBuilder(Piece piece)
	{
		this.piece = piece;
		team = piece.Team;
		actions = new List<Vector2Int>();
	}

	public static ActionBuilder For(Piece piece)
	{
		return new ActionBuilder(piece);
	}

	public ActionBuilder Straight(int spaces, ActionOption actionOptions)
	{
		StraightDirections
			.ForEach(direction => ApplyDirection(spaces, direction, actionOptions));
		return this;
	}

	public ActionBuilder Diagonal(int spaces, ActionOption actionOptions)
	{
		DiagonalDirections
			.ForEach(direction => ApplyDirection(spaces, direction, actionOptions));
		return this;
	}

	public ActionBuilder AnyDirection(int spaces, ActionOption actionOptions)
	{
		Straight(spaces, actionOptions);
		Diagonal(spaces, actionOptions);
		return this;
	}

	private void ApplyDirection(int spaces, Vector2Int direction, ActionOption actionOptions)
	{
		isBlocked = false;
		for (int i = 1; i <= spaces; i++)
		{
			currentAction = piece.BoardPosition + direction * i;
			if (BoardController.IsWithinBoard(currentAction) && (actionOptions == ActionOption.None
			    || actionOptions.GetFlags()
					.All(actionOption => ActionValidations[actionOption].Invoke(this))))
			{
				actions.Add(currentAction);
			}
			else
			{
				isBlocked = true;
			}
		}
	}
	
	public List<Vector2Int> Build()
	{
		return actions;
	}

}
