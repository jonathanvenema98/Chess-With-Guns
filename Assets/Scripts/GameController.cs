using UnityEngine;

public class GameController : Singleton<GameController>
{
	[SerializeField] private Obstacle boardItem;
	[SerializeField] private Vector2Int target;
	
	public static int TurnCount { get; private set; }
	
	public static Team CurrentTeam { get; private set; }

	public static bool FirstTurn
	{
		get { return TurnCount == 0; }
	}
	
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
		// 	.ForwardUntilBlocked(-1)
		// 	.DiagonallyLeftUntilBlocked(1)
		// 	.DiagonallyRightUntilBlocked(1)
		// 	.Build()
		// 	.ForEach(m => Debug.Log(m));
	}
	
	public static void NextTurn()
	{
		TurnCount++;
	}

	public static void SetCurrentTeam(Team team)
	{
		CurrentTeam = team;
		if (CurrentTeam == Team.White)
			NextTurn();
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
