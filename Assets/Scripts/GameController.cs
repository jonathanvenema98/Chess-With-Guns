using Unity.Collections;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	
	[SerializeField] private Obstacle boardItem;
	[SerializeField] private Vector2Int target;
	[SerializeField] private Color focusedTileColour;
	[SerializeField] private GameMode gameMode;
	
	public static int Round { get; private set; }
	
	public static Team CurrentTeam { get; private set; }

	public static bool FirstRound => Round == 1;

	public static Color FocusedTileColour => Instance.focusedTileColour;

	public static GameMode GameMode => Instance.gameMode;

	[InspectorButton]
	private void MoveToTarget()
	{
		//For testing purposes
		BoardController.MoveBoardItemTo(boardItem, target);
	}

	[InspectorButton]
	private void LogMoves()
	{
		//For testing purposes
		// MoveBuilder.For(boardItem)
		// 	.WhereNot(MoveOptions.BoardItem)
		// 	.ForwardUntilBlocked(2)
		// 	.DiagonallyLeftUntilBlocked(1)
		// 	.DiagonallyRightUntilBlocked(1)
		// 	.Build()
		// 	.ForEach(m => Debug.Log(m));
	}

	[InspectorButton]
	private void BoardPosition()
	{
		Debug.Log(BoardController.WorldPositionToBoardPosition(boardItem.transform.position));
	}
	
	public static void NextRound()
	{
		Round++;
	}

	public static void SetCurrentTeam(Team team)
	{
		CurrentTeam = team;
		if (CurrentTeam == Team.White)
			NextRound();
	}

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
			
	}
}
