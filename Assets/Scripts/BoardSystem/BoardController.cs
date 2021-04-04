using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : Singleton<BoardController>
{
	[SerializeField] private Vector2Int boardSize;
	
	[SerializeField] private Tilemap boardTilemap;
	[SerializeField] private Tile tileA;
	[SerializeField] private Tile tileB;

	private Vector2Int worldPositionOffset;
	private IBoardItem[,] board;
	private int boardLength;
	
	public static Vector2Int BoardSize
	{
		get
		{
			return Instance.boardSize;
		}
	}

	public static int BoardLength
	{
		get
		{
			return Instance.boardLength;
		}
	}
	
	// Use this for initialization
	private void Start ()
	{
		UpdateBoard();
		board = new IBoardItem[boardSize.x, boardSize.y];
		boardLength = Mathf.Max(boardSize.x, boardSize.y);
	}

	[InspectorButton]
	private void UpdateBoard()
	{
		worldPositionOffset = new Vector2Int(boardSize.x / 2, boardSize.y / 2);
		
		if (tileA == null || tileB == null || boardTilemap == null)
		{
			Debug.LogWarning("Make sure all the necessary assets are set in the inspector before trying to update the board");
			return;
		}

		//The board is already the correct size
		if (boardTilemap.size.x == boardSize.x && boardTilemap.size.y == boardSize.y)
			return;

		//Clear the previous board
		boardTilemap.ClearAllTiles();

		for (int x = 0; x < boardSize.x; x++)
		{
			for (int y = 0; y < boardSize.y; y++)
			{
				Vector3Int tilePosition = new Vector3Int(x - worldPositionOffset.x, y - worldPositionOffset.y, 0);
				Tile tile = DetermineTile(x, y);
				
				boardTilemap.SetTile(tilePosition, tile);
			}
		}
		boardTilemap.ResizeBounds();
	}

	private Tile DetermineTile(int x, int y)
	{
		return y % 2 == x % 2
			? tileA
			: tileB;
	}

	public static IBoardItem GetBoardItemAt(Vector2Int position)
	{
		return IsWithinBoard(position)
			? Instance.board[position.x, position.y]
			: null;
	}

	public static T GetBoardItemAt<T>(Vector2Int position) where T: IBoardItem
	{
		IBoardItem boardItem = GetBoardItemAt(position);
		if (boardItem is T)
			return (T) boardItem;
		
		return default(T);
	}

	public static bool IsBoardItemAt(Vector2Int position)
	{
		return GetBoardItemAt(position) != null;
	}

	public static bool IsObstacleAt(Vector2Int position)
	{
		return IsBoardItemAt(position) && GetBoardItemAt(position) is Obstacle;
	}

	public static bool IsPieceAt(Vector2Int position)
	{
		return IsBoardItemAt(position) && GetBoardItemAt(position) is IPiece;
	}

	public static bool IsWithinBoard(Vector2Int position)
	{
		return Application.isPlaying
			&& position.x >= 0 && position.x < BoardSize.x
			&& position.y >= 0 && position.y < BoardSize.y;
	}

	public static Vector3 BoardPositionToWorldPosition(Vector2Int boardPosition)
	{
		return new Vector3(
			boardPosition.x - Instance.worldPositionOffset.x + 0.5F,
			boardPosition.y - Instance.worldPositionOffset.y + 0.5F);
	}
	
	public static bool MoveBoardItemTo<T>(T boardItem, Vector2Int to) where T: IBoardItem
	{
		if (IsBoardItemAt(to) || !IsWithinBoard(to))
			return false;
		
		RemoveBoardItemAt(boardItem.BoardPosition);
		SetBoardItemAt(boardItem, to);
		return true;
	}

	public static bool IsFriendlyAt(Vector2Int boardPosition, Team playerTeam)
	{
		IPiece piece = GetBoardItemAt<IPiece>(boardPosition);
		return piece != null && piece.Team == playerTeam;
	}
	
	public static bool IsEnemyAt(Vector2Int boardPosition, Team playerTeam)
	{
		IPiece piece = GetBoardItemAt<IPiece>(boardPosition);
		return piece != null && piece.Team != playerTeam;
	}

	private static void RemoveBoardItemAt(Vector2Int position)
	{
		Instance.board[position.x, position.y] = null;
	}

	private static void SetBoardItemAt<T>(T boardItem, Vector2Int position) where T: IBoardItem
	{
		Instance.board[position.x, position.y] = boardItem;
		boardItem.BoardPosition = position;
		boardItem.Transform.position = BoardPositionToWorldPosition(position);
	}
}
