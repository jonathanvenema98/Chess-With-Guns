using System.Linq;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	
	[SerializeField] private Piece piece;
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
		if (piece.GetMoves().Contains(target))
		{
			BoardController.MoveBoardItemTo(piece, target);
		}
	}

	[InspectorButton]
	private void LogMoves()
	{
		ActionBuilder.For(piece)
			.Straight(1, ActionOption.IsUnoccupied | ActionOption.ValidTerrainType | ActionOption.UntilBlocked)
			.Build()
			.ForEach(action => Debug.Log(action));

	}

	public static void NextRound()
	{
		Round++;
	}

	public static void SetCurrentTeam(Team team)
	{
		CurrentTeam = team;
		if (CurrentTeam == Team.Blue)
			NextRound();
	}

	private new void Awake()
	{
		base.Awake();
		
		BoardController.Instance.Initialise();
		BoardController.Instance.LoadLevel("Symmetrical Level");
	}

	// Use this for initialization
	private void Start ()
	{
	}
	
	// Update is called once per frame
	private void Update ()
	{
        // if (Input.GetMouseButtonDown(Utils.LeftMouseButton))
        // {
        // 	Vector2Int boardPosition = BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition);
        // 	if (piece.GetMoves().Contains(boardPosition))
        // 	{
        // 		BoardController.MoveBoardItemTo(piece, boardPosition);
        // 	}
        // }


        //    Example function call
        if (Input.GetMouseButtonDown(Utils.RightMouseButton))
        {
            //Debug.Log(BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition));
            FadingUIManager.Instance.CreateFadingText(
                BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition),
                "Red text message", Color.red);
        }
        if(Input.GetMouseButtonDown(Utils.LeftMouseButton))
		{
			//Debug.Log(BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition));
			FadingUIManager.Instance.CreateFadingText(
				BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition),
				"Blue 2s text message", Color.blue, 2.0f);
		}
	}
}
