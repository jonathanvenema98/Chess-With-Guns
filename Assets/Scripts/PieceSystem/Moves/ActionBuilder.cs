using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionBuilder
{
	private readonly IPiece piece;
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

	private ActionBuilder(IPiece piece)
	{
		this.piece = piece;
		team = piece.Team;
		actions = new List<Vector2Int>();
	}

	public static ActionBuilder For(IPiece piece)
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
			currentAction = piece.BoardPosition + direction * spaces;
			if (actionOptions == ActionOption.None  ||
			    actionOptions.GetFlags()
					.All(actionOption => ActionValidations[actionOption].Invoke(this)))
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


	// public ActionBuilder ForwardUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	Vector2Int forward = RelativeDirection(Vector2Int.up);
	// 	UntilBlockedMovement(spaces, actionOption, forward);
	//
	// 	return this;
	// }
	//
	// public ActionBuilder HorizontalUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	LeftUntilBlocked(spaces, actionOption);
	// 	RightUntilBlocked(spaces, actionOption);
	//
	// 	return this;
	// }
	//
	// public ActionBuilder LeftUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	Vector2Int left = RelativeDirection(Vector2Int.left);
	// 	UntilBlockedMovement(spaces, actionOption, left);
	//
	// 	return this;
	// }
	//
	// public ActionBuilder RightUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	Vector2Int right = RelativeDirection(Vector2Int.right);
	// 	UntilBlockedMovement(spaces, actionOption, right);
	//
	// 	return this;
	// }
	//
	// public ActionBuilder DiagonallyLeftUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	Vector2Int diagonal = RelativeDirection(Vector2Int.left + Vector2Int.up);
	// 	UntilBlockedMovement(spaces, actionOption, diagonal);
	//
	// 	return this;
	// }
	//
	// public ActionBuilder DiagonallyRightUntilBlocked(int spaces, ActionOption actionOption = ActionOption.None)
	// {
	// 	Vector2Int diagonal = RelativeDirection(Vector2Int.right + Vector2Int.up);
	// 	UntilBlockedMovement(spaces, actionOption, diagonal);
	//
	// 	return this;
	// }
	//
	// private Vector2Int RelativeDirection(Vector2Int direction)
	// {
	// 	return piece.Team == Team.Red
	// 		? direction * -1
	// 		: direction;
	// }
	//
	// private void UntilBlockedMovement(int spaces, ActionOption actionOption, Vector2Int direction)
	// {
	// 	if (spaces == -1) //Move an unlimited number of spaces in that direction until blocked
	// 		spaces = BoardController.BoardLength;
	// 	
	// 	for (int i = 1; i <= spaces; i++)
	// 	{
	// 		Vector2Int move = piece.BoardPosition + direction * i;
	// 		if (!AddMove(move, ActionOption | actionOption, MoveOptionsAreTrue))
	// 			break;
	// 	}
	// }
	//
	// //This is a bit of a mess, so if anyone has a better way to do this, please go ahead!
	// private bool IsValid(Vector2Int move, ActionOption actionOption, bool moveOptionIsTrue)
	// {
	// 	if (actionOption.HasFlag(ActionOption.IsEnemy) &&
	// 	    BoardController.IsEnemyAt(move, piece.Team) != moveOptionIsTrue)
	// 	{
	// 		return false;
	// 	}
	//
	// 	if (actionOption.HasFlag(ActionOption.IsFriendly) &&
	// 	    BoardController.IsFriendlyAt(move, piece.Team) != moveOptionIsTrue)
	// 	{
	// 		return false;
	// 	}
	//
	// 	if (actionOption.HasFlag(ActionOption.IsObstacle) && BoardController.IsObstacleAt(move) != moveOptionIsTrue)
	// 	{
	// 		return false;
	// 	}
	//
	// 	if (!BoardController.IsWithinBoard(move))
	// 	{
	// 		return false;
	// 	}
	//
	// 	return true;
	// }
	//
	// private bool AddMove(Vector2Int move, ActionOption actionOption, bool moveOptionIsTrue)
	// {
	// 	if (IsValid(move, actionOption, moveOptionIsTrue))
	// 	{
	// 		actions.Add(move);
	// 		return true;
	// 	}
	//
	// 	return false;
	// }

}
