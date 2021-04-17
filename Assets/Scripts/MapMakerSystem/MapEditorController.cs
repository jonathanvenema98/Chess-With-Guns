using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

using UnityTileData = UnityEngine.Tilemaps.TileData;

[ExecuteAlways]
public class MapEditorController : Singleton<MapEditorController>, ISaveLevelDriver, ILoadLevelDriver
{
    [SerializeField] private Transform pieceParent;
    
    private List<TileData> changedTiles;
    private Dictionary<Vector3Int, Piece> pieces;

    private bool MadeChanges => changedTiles.Count != 0;
    private string Filename => $"{GetInstanceID()}";
    public Tilemap Tilemap { get; private set; }

    public static BrushMode CurrentBrushMode { get; set; }
    public static EditMode CurrentEditMode { get; set; }
    public static HeightTile CurrentTile { get; set; }
    public static Piece CurrentPiece { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        BoardController.Instance.Initialise();
        Tilemap = BoardController.BoardTilemap;
        
        if (Application.isPlaying)
        {
            changedTiles = new List<TileData>();
            pieces = new Dictionary<Vector3Int, Piece>();
        }
        else if (Application.isEditor && SaveSystem.FileExists(Filename))
        {
            ApplyChanges();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(Utils.LeftMouseButton) && !Utils.IsMouseOverUI())
        {
            switch (CurrentEditMode)
            {
                case EditMode.Tiles:
                    PlaceTile();
                    break;
                case EditMode.Pieces:
                    PlacePiece();
                    break;
            }
        }
    }

    private void PlaceTile()
    {
        var cellPosition = BoardController.WorldPositionToCellPosition(Utils.MouseWorldPosition);
        
        TileBase tile = CurrentBrushMode == BrushMode.Paint
            ? CurrentTile : null;

        if (Tilemap.GetTile(cellPosition) == tile)
            return;
        SetTile(cellPosition, tile);
    }

    private void PlacePiece()
    {
        var cellPosition = BoardController.WorldPositionToCellPosition(Utils.MouseWorldPosition);
        if (CurrentBrushMode == BrushMode.Paint)
        {
            if (pieces.TryGetValue(cellPosition, out Piece piece))
            {
                if (piece == CurrentPiece)
                    return;

                Destroy(piece.gameObject);
            }

            pieces[cellPosition] = CurrentPiece.Instantiate(pieceParent);
            pieces[cellPosition].BoardPosition = BoardController.V3ToV2(cellPosition);
            pieces[cellPosition].transform.position = BoardController.CellPositionToWorldPosition(cellPosition);
        }
        else if (pieces.TryGetValue(cellPosition, out Piece piece))
        {
            pieces.Remove(cellPosition);
            Destroy(piece.gameObject);
        }
    }

    #region LoadSaveLevelDrivers

    [InspectorButton]
    public void ClearLevel()
    {
        changedTiles.Clear();
        changedTiles.Add(TileData.ClearTilemap);
        Tilemap.ClearAllTiles();
        
        pieces.ForEach(pair => Destroy(pair.Value.gameObject));
        pieces.Clear();
    }

    public void SetTile(Vector3Int cellPosition, TileBase tile)
    {
        Tilemap.SetTile(cellPosition, tile);
        string tileName = tile == null ? null : tile.name;
        changedTiles.Add(new TileData(cellPosition, tileName));
    }

    public IEnumerable<Piece> GetPieces()
    {
        return pieces.Values.ToList();
    }

    public void SetPiece(Vector3Int cellPosition, Piece piece)
    {
        pieces[cellPosition] = piece.Instantiate(pieceParent);
        pieces[cellPosition].BoardPosition = BoardController.V3ToV2(cellPosition);
        pieces[cellPosition].transform.position = BoardController.CellPositionToWorldPosition(cellPosition);
    }

    #endregion

    private void ApplyChanges()
    {
        if (Tilemap == null)
            Tilemap = GetComponentInChildren<Tilemap>();
        var tiles = SaveSystem.CreateTileMappings();

        var tilemapData = SaveSystem.LoadData<TilemapData>(Filename);
        foreach (var tileData in tilemapData.Tiles)
        {
            if (Equals(tileData, TileData.ClearTilemap))
                Tilemap.ClearAllTiles();
            else
            {
                Tilemap.SetTile(tileData.CellPosition,
                    string.IsNullOrEmpty(tileData.Name) ? null : tiles[tileData.Name]);
            }
        }

        Tilemap.CompressBounds();

        SaveSystem.DeleteFile(Filename);
        SaveScene();
    }

    private void SaveScene()
    {
        EditorUtility.SetDirty(Tilemap); //Notifies the game that there was a change to the tilemap
        Debug.Log($"Scene saved successfully: {EditorSceneManager.SaveOpenScenes()}"); //Automatically saves the scene

    }

    private void SaveChanges()
    {
        SaveSystem.SaveData(Filename, new TilemapData(changedTiles));
        Debug.Log("Saved");
    }

    private void OnApplicationQuit()
    {
        if (MadeChanges)
        {
            SaveChanges();
        }
    }
}

public enum EditMode
{
    Tiles, Pieces
}

public enum BrushMode
{
    Paint, Erase
}