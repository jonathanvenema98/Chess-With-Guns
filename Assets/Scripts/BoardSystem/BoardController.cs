using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : Singleton<BoardController>
{
	[SerializeField] private Vector2Int boardSize;
	
	[SerializeField] private Tilemap boardTilemap;
	[SerializeField] private Tile tileA;
	[SerializeField] private Tile tileB;
	
	public Vector2Int BoardSize
	{
		get
		{
			return boardSize;
		}
	}

	[InspectorButton]
	private void UpdateBoard()
	{
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
		Vector2Int bottomLeftCorner = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);

		for (int x = 0; x < boardSize.x; x++)
		{
			for (int y = 0; y < boardSize.y; y++)
			{
				Vector3Int tilePosition = new Vector3Int(bottomLeftCorner.x + x, bottomLeftCorner.y + y, 0);
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
	
	// Use this for initialization
	private void Start ()
	{
		UpdateBoard();
	}
	
	// Update is called once per frame
	private void Update () {
		
	}
}
