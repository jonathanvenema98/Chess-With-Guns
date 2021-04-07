using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : Singleton<BoardController>
{
	[SerializeField] private Vector2Int boardSize;

	[SerializeField] private int unitsPerTile = 4;
	[SerializeField] private Tilemap boardTilemap;
	[SerializeField] private Tile tileA;
	[SerializeField] private Tile tileB;

	[SerializeField] private Transform tileBordersParent;
	[SerializeField] private SpriteRenderer tileBorderPrefab;

	private Vector2Int tilePositionOffset;
	private IBoardItem[,] board;
	private int boardLength;
	private readonly Dictionary<Vector2Int, SpriteRenderer> tileBorders = new Dictionary<Vector2Int, SpriteRenderer>();

	public static Vector2Int BoardSize => Instance.boardSize;

	public static int BoardLength => Instance.boardLength; 
	
	public static int UnitsPerTile => Instance.unitsPerTile; 
	

	public static float HalfUnitsPerTile => UnitsPerTile / 2F;

	public static Vector2 BottomLeftCorner =>
		new Vector2(
			-Instance.tilePositionOffset.x * UnitsPerTile,
			-Instance.tilePositionOffset.y * UnitsPerTile);

	public static Vector2 TopRightCorner =>
		new Vector2(
			(BoardSize.x - Instance.tilePositionOffset.x) * UnitsPerTile,
			(BoardSize.y - Instance.tilePositionOffset.y) * UnitsPerTile);

	// Use this for initialization
	private new void Awake()
	{
		base.Awake();
		
		UpdateBoard();
		board = new IBoardItem[boardSize.x, boardSize.y];
		boardLength = Mathf.Max(boardSize.x, boardSize.y);
	}

	[InspectorButton]
	private void UpdateBoard()
	{
		tilePositionOffset = new Vector2Int(boardSize.x / 2, boardSize.y / 2);

		if (tileA == null || tileB == null || boardTilemap == null)
		{
			Debug.LogWarning(
				"Make sure all the necessary assets are set in the inspector before trying to update the board");
			return;
		}

		//The board is already the correct size
		if (boardTilemap.size.x / UnitsPerTile == boardSize.x && boardTilemap.size.y / UnitsPerTile == boardSize.y)
			return;

		//Clear the previous board
		boardTilemap.ClearAllTiles();
		boardTilemap.layoutGrid.cellSize = new Vector3(1, 1) * unitsPerTile;

		for (int x = 0; x < boardSize.x; x++)
		{
			for (int y = 0; y < boardSize.y; y++)
			{
				Vector3Int tilePosition = new Vector3Int(x - tilePositionOffset.x, y - tilePositionOffset.y, 0);
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

	public static IBoardItem GetBoardItemAt(Vector2Int boardPosition)
	{
		return IsWithinBoard(boardPosition)
			? Instance.board[boardPosition.x, boardPosition.y]
			: null;
	}

	public static T GetBoardItemAt<T>(Vector2Int boardPosition) where T : IBoardItem
	{
		IBoardItem boardItem = GetBoardItemAt(boardPosition);
		if (boardItem is T t)
			return t;

		return default;
	}

	public static bool IsBoardItemAt(Vector2Int boardPosition)
	{
		return GetBoardItemAt(boardPosition) != null;
	}

	public static bool IsObstacleAt(Vector2Int boardPosition)
	{
		return IsBoardItemAt(boardPosition) && GetBoardItemAt(boardPosition) is Obstacle;
	}

	public static bool IsPieceAt(Vector2Int boardPosition)
	{
		return IsBoardItemAt(boardPosition) && GetBoardItemAt(boardPosition) is IPiece;
	}

	public static bool IsWithinBoard(Vector2Int boardPosition)
	{
		return Application.isPlaying
		       && boardPosition.x >= 0 && boardPosition.x < BoardSize.x
		       && boardPosition.y >= 0 && boardPosition.y < BoardSize.y;
	}

	public static Vector3 BoardPositionToWorldPosition(Vector2Int boardPosition)
	{
		return new Vector3(
			(boardPosition.x - Instance.tilePositionOffset.x) * UnitsPerTile + HalfUnitsPerTile,
			(boardPosition.y - Instance.tilePositionOffset.y) * UnitsPerTile + HalfUnitsPerTile);
	}

	//To be tested:
	public static Vector2Int WorldPositionToBoardPosition(Vector2 worldPosition)
	{
		return new Vector2Int(
			Mathf.FloorToInt(worldPosition.x / UnitsPerTile) + Instance.tilePositionOffset.x,
			Mathf.FloorToInt(worldPosition.y / UnitsPerTile) + Instance.tilePositionOffset.y);
	}

	public static bool MoveBoardItemTo<T>(T boardItem, Vector2Int toBoardPosition) where T: IBoardItem
	{
		if (IsBoardItemAt(toBoardPosition) || !IsWithinBoard(toBoardPosition))
			return false;
		
		RemoveBoardItemAt(boardItem.BoardPosition);
		SetBoardItemAt(boardItem, toBoardPosition);
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

	private static void RemoveBoardItemAt(Vector2Int boardPosition)
	{
		Instance.board[boardPosition.x, boardPosition.y] = null;
	}

	private static void SetBoardItemAt<T>(T boardItem, Vector2Int boardPosition) where T: IBoardItem
	{
		Instance.board[boardPosition.x, boardPosition.y] = boardItem;
		boardItem.BoardPosition = boardPosition;
		boardItem.Transform.position = BoardPositionToWorldPosition(boardPosition);
	}

	public static void HideBorderAt(Vector2Int boardPosition)
	{
		if (Instance.tileBorders.TryGetValue(boardPosition, out var tileBorder))
		{
			tileBorder.enabled = false;
		}
	}
	
	public static void DestroyBorderAt(Vector2Int boardPosition)
	{
		if (Instance.tileBorders.TryGetValue(boardPosition, out var tileBorder))
		{
			Destroy(tileBorder.gameObject);
			Instance.tileBorders.Remove(boardPosition);
		}
	}

	public static void ShowBorderAt(Vector2Int boardPosition, Color colour)
	{
		var tileBorders = Instance.tileBorders;

		if (!tileBorders.TryGetValue(boardPosition, out var tileBorder))
		{
			 tileBorder = Instantiate(
				Instance.tileBorderPrefab,
				BoardPositionToWorldPosition(boardPosition),
				Quaternion.identity,
				Instance.tileBordersParent);

			 tileBorders[boardPosition] = tileBorder;
		}

		tileBorder.enabled = true;
		tileBorder.color = colour;
	}

	public static void DestroyAllBorders()
	{
		var tileBorders = Instance.tileBorders;
		foreach (var pair in tileBorders)
		{
			Destroy(pair.Value.gameObject);
			tileBorders.Remove(pair.Key);
		}
	}
}
