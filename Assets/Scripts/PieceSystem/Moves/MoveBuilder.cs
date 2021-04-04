using System.Collections.Generic;
using UnityEngine;

public class MoveBuilder
{
	private IPiece piece;
	private List<Vector2Int> moves;

	private MoveOptions MoveOptions { get; set; }
	private bool MoveOptionsAreTrue { get; set; }
	
	private MoveBuilder(IPiece piece)
	{
		this.piece = piece;
		moves = new List<Vector2Int>();
	}

	public static MoveBuilder For(IPiece piece)
	{
		return new MoveBuilder(piece);
	}

	public MoveBuilder Where(MoveOptions moveOptions)
	{
		MoveOptions = moveOptions;
		MoveOptionsAreTrue = true;

		return this;
	}

	public MoveBuilder WhereNot(MoveOptions moveOptions)
	{
		MoveOptions = moveOptions;
		MoveOptionsAreTrue = false;

		return this;
	}

	public MoveBuilder ForwardUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		Vector2Int forward = RelativeDirection(Vector2Int.up);
		UntilBlockedMovement(spaces, moveOptions, forward);

		return this;
	}

	public MoveBuilder HorizontalUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		LeftUntilBlocked(spaces, moveOptions);
		RightUntilBlocked(spaces, moveOptions);

		return this;
	}

	public MoveBuilder LeftUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		Vector2Int left = RelativeDirection(Vector2Int.left);
		UntilBlockedMovement(spaces, moveOptions, left);

		return this;
	}
	
	public MoveBuilder RightUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		Vector2Int right = RelativeDirection(Vector2Int.right);
		UntilBlockedMovement(spaces, moveOptions, right);

		return this;
	}

	public MoveBuilder DiagonallyLeftUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		Vector2Int diagonal = RelativeDirection(Vector2Int.left + Vector2Int.up);
		UntilBlockedMovement(spaces, moveOptions, diagonal);

		return this;
	}
	
	public MoveBuilder DiagonallyRightUntilBlocked(int spaces, MoveOptions moveOptions = MoveOptions.None)
	{
		Vector2Int diagonal = RelativeDirection(Vector2Int.right + Vector2Int.up);
		UntilBlockedMovement(spaces, moveOptions, diagonal);

		return this;
	}

	public List<Vector2Int> Build()
	{
		return moves;
	}
	
	private Vector2Int RelativeDirection(Vector2Int direction)
	{
		return piece.Team == Team.Black
			? direction * -1
			: direction;
	}

	private void UntilBlockedMovement(int spaces, MoveOptions moveOptions, Vector2Int direction)
	{
		if (spaces == -1) //Move an unlimited number of spaces in that direction until blocked
			spaces = BoardController.BoardLength;
		
		for (int i = 1; i <= spaces; i++)
		{
			Vector2Int move = piece.BoardPosition + direction * i;
			if (!AddMove(move, MoveOptions | moveOptions, MoveOptionsAreTrue))
				break;
		}
	}

	//This is a bit of a mess, so if anyone has a better way to do this, please go ahead!
	private bool IsValid(Vector2Int move, MoveOptions moveOptions, bool moveOptionIsTrue)
	{
		if (moveOptions.HasFlag(MoveOptions.FirstTurn) && !GameController.FirstTurn)
		{
			return false;
		}

		if (moveOptions.HasFlag(MoveOptions.Enemy) &&
		    BoardController.IsEnemyAt(move, piece.Team) != moveOptionIsTrue)
		{
			return false;
		}

		if (moveOptions.HasFlag(MoveOptions.Friendly) &&
		    BoardController.IsFriendlyAt(move, piece.Team) != moveOptionIsTrue)
		{
			return false;
		}

		if (moveOptions.HasFlag(MoveOptions.Obstacle) && BoardController.IsObstacleAt(move) != moveOptionIsTrue)
		{
			return false;
		}

		if (!BoardController.IsWithinBoard(move))
		{
			return false;
		}

		return true;
	}
	
	private bool AddMove(Vector2Int move, MoveOptions moveOptions, bool moveOptionIsTrue)
	{
		if (IsValid(move, moveOptions, moveOptionIsTrue))
		{
			moves.Add(move);
			return true;
		}

		return false;
	}

}
