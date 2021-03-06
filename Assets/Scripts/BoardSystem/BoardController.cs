using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : Singleton<BoardController>, ILoadLevelDriver
{
	[SerializeField] private int unitsPerTile;
	[SerializeField] private Tilemap boardTilemap;

	[SerializeField] private Transform pieceParent;
	[SerializeField] private Transform tileBordersParent;
	[SerializeField] private SpriteRenderer tileBorderPrefab;

	private IBoardItem[,] board;
	private readonly Dictionary<Vector2Int, SpriteRenderer> tileBorders = new Dictionary<Vector2Int, SpriteRenderer>();

	public static Vector2Int BoardSize { get; private set; }
	public static int BoardLength { get; private set; }
	public static int UnitsPerTile { get; private set; }
	public static float HalfUnitsPerTile { get; private set; }
	public static Vector3 WorldOffset { get; private set; }
	public static Tilemap BoardTilemap { get; private set; }

	public void Initialise()
	{
		UnitsPerTile = Instance.unitsPerTile;
		HalfUnitsPerTile = UnitsPerTile / 2F;
		WorldOffset = Vector3.up * HalfUnitsPerTile / 4F;
		BoardTilemap = boardTilemap;
	}

	#region Level Driver

	public void LoadLevel(string level)
	{
		SaveSystem.LoadLevel(this, level, levelData =>
		{
			var boardSize = V3ToV2(levelData.TilemapSize);

			board = new IBoardItem[boardSize.x, boardSize.y];
			int boardLength = Mathf.Max(boardSize.x, boardSize.y);

			BoardSize = boardSize;
			BoardLength = boardLength;
		});
	}

	public void ClearLevel()
	{
		boardTilemap.ClearAllTiles();
	}

	public void SetTile(Vector3Int cellPosition, TileBase tile)
	{
		boardTilemap.SetTile(cellPosition, tile);
	}

	public void SetPiece(Vector3Int cellPosition, Piece piece)
	{
		SetBoardItemAt(piece.Instantiate(pieceParent), CellPositionToBoardPosition(cellPosition));
	}

	#endregion

	#region Board Functions

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
		return IsBoardItemAt(boardPosition) && GetBoardItemAt(boardPosition) is Piece;
	}

	public static bool IsWithinBoard(Vector2Int boardPosition)
	{
		return Application.isPlaying && BoardTilemap.HasTile(BoardPositionToCellPosition(boardPosition));
	}

	public static Vector3 BoardPositionToWorldPosition(Vector2Int boardPosition)
	{
		return BoardTilemap.CellToWorld(BoardPositionToCellPosition(boardPosition))
		       + WorldOffset + GetWorldHeightOffsetAt(boardPosition);
	}

	public static Vector2Int WorldPositionToBoardPosition(Vector3 worldPosition)
	{
		return CellPositionToBoardPosition(BoardTilemap.WorldToCell(worldPosition));
	}

	public static Vector3 CellPositionToWorldPosition(Vector3Int cellPosition)
	{
		return BoardTilemap.CellToWorld(cellPosition);
	}

	public static Vector2Int V3ToV2(Vector3Int v)
	{
		return new Vector2Int(v.x, v.y);
	}
	
	public static Vector3Int V2ToV3(Vector2Int v)
	{
		return new Vector3Int(v.x, v.y, 0);
	}

	/// <summary>
	/// Converts a position on the board (Which has 0,0 in the bottom left hand corner) to its position on the tilemap
	/// (Which has 0,0 in the centre)
	/// </summary>
	/// <param name="boardPosition">The position on the board</param>
	/// <returns>The position on the tilemap</returns>
	public static Vector3Int BoardPositionToCellPosition(Vector2Int boardPosition)
	{
		return V2ToV3(boardPosition) + BoardTilemap.origin;
	}

	/// <summary>
	/// Converts a position on the tilemap (Which has 0,0 in the centre) to its position on the board (Which has 0,0 in
	/// the bottom left hand corner)
	/// </summary>
	/// <param name="cellPosition">The position on the tilemap</param>
	/// <returns>The position on the board</returns>
	public static Vector2Int CellPositionToBoardPosition(Vector3Int cellPosition)
	{
		return V3ToV2(cellPosition - BoardTilemap.origin);
	}
	
	/// <summary>
	/// Gets the cell position of a world position. Unlike calling Tilemap#WorldToCell directly, this sets the z
	/// position to 0, to prevent floating tiles.
	/// </summary>
	/// <param name="worldPosition">The world position</param>
	/// <returns>The cell position, with 0 for the z position</returns>
	public static Vector3Int WorldPositionToCellPosition(Vector3 worldPosition)
	{
		var cellPosition = BoardTilemap.WorldToCell(worldPosition);
		cellPosition.z = 0;
		return cellPosition;
	}

	public static void MoveBoardItemTo<T>(T boardItem, Vector2Int toBoardPosition) where T: IBoardItem
	{
		if (IsBoardItemAt(toBoardPosition) || !IsWithinBoard(toBoardPosition))
			return;
		
		RemoveBoardItemAt(boardItem.BoardPosition);
		SetBoardItemAt(boardItem, toBoardPosition);
		if (boardItem is Piece piece)
		{
			piece.OnPieceMove();
		}
	}

	public static bool IsFriendlyAt(Vector2Int boardPosition, Team playerTeam)
	{
		Piece piece = GetBoardItemAt<Piece>(boardPosition);
		return piece != null && piece.Team == playerTeam;
	}
	
	public static bool IsEnemyAt(Vector2Int boardPosition, Team playerTeam)
	{
		Piece piece = GetBoardItemAt<Piece>(boardPosition);
		return piece != null && piece.Team != playerTeam;
	}

	public static void RemoveBoardItemAt(Vector2Int boardPosition)
	{
		Instance.board[boardPosition.x, boardPosition.y] = null;
	}

	public static void SetBoardItemAt<T>(T boardItem, Vector2Int boardPosition) where T: IBoardItem
	{
		Instance.board[boardPosition.x, boardPosition.y] = boardItem;
		boardItem.BoardPosition = boardPosition;
		boardItem.Transform.position = BoardPositionToWorldPosition(boardPosition);
	}

	/// <summary>
	/// Returns the world height offset of the tile at the board position (E.g: half tiles aren't as tall as full tiles
	/// so have a slight negative height offset) 
	/// </summary>
	/// <param name="boardPosition">The board position of the tile</param>
	/// <returns>The world height offset of the tile</returns>
	public static Vector3 GetWorldHeightOffsetAt(Vector2Int boardPosition)
	{
		var cellPosition = BoardPositionToCellPosition(boardPosition);
		if (BoardTilemap.HasTile(cellPosition))
		{
			var heightTile = BoardTilemap.GetTile<HeightTile>(cellPosition);
			return Vector3.up * (((int) heightTile.Height - Utils.PixelsPerUnit) * Utils.Pixel);
		}

		return Vector3.zero;
	}

	public static bool CanPieceMoveTo(Piece piece, Vector2Int toBoardPosition)
	{
		var cellPosition = BoardPositionToCellPosition(toBoardPosition);
		return BoardTilemap.HasTile(cellPosition)
		       && piece.AcceptableTerrainTypes.Contains(BoardTilemap.GetTile<HeightTile>(cellPosition).TerrainType);
	}
	
	#endregion

	#region Borders

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

	#endregion
	
}
