using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

using UnityTileData = UnityEngine.Tilemaps.TileData;

[ExecuteAlways]
public class PlaymodeTilemapEditor : Singleton<PlaymodeTilemapEditor>, ITilemapDriver
{
#if UNITY_EDITOR
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private string levelName;
    
    
    private readonly List<TileData> changedTiles = new List<TileData>();

    private bool MadeChanges => changedTiles.Count != 0;
    private string Filename => $"{tilemap.name}-{GetInstanceID()}";
    public Tilemap Tilemap => tilemap;
    

    // Start is called before the first frame update
    private void Start()
    {
        if (tilemap == null)
            return;

        if (Application.isPlaying)
        {
            Tilemap.tilemapTileChanged += TilemapTileChangedSubscriber;
        }
        else if (Application.isEditor && SaveSystem.FileExists(Filename))
        {
            ApplyChanges();
        }
    }

    private void TilemapTileChangedSubscriber(Tilemap changedTilemap, Tilemap.SyncTile[] syncTiles)
    {
        if (changedTilemap != tilemap) return;

        if (syncTiles.Length > 0)
        {
            foreach (var syncTile in syncTiles)
            {
                string tileName = syncTile.tile == null ? null : syncTile.tile.name;
                var tileData = new TileData(syncTile.position, tileName);
                changedTiles.Add(tileData);
            }
        }
    }
    
    [InspectorButton]
    public void ClearTilemap()
    {
        changedTiles.Clear();
        changedTiles.Add(TileData.ClearTilemap);
        tilemap.ClearAllTiles();
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
        string tileName = tile == null ? null : tile.name;
        changedTiles.Add(new TileData(position, tileName));
    }
    

    [InspectorButton]
    private void ApplyChanges()
    {
        if (tilemap == null)
            tilemap = GetComponentInChildren<Tilemap>();
        var tiles = SaveSystem.GetTilePalette();

        var tilemapData = SaveSystem.LoadData<TilemapData>(Filename);
        foreach (var tileData in tilemapData.Tiles)
        {
            if (Equals(tileData, TileData.ClearTilemap))
                tilemap.ClearAllTiles();
            else
            {
                tilemap.SetTile(tileData.TilePosition,
                    string.IsNullOrEmpty(tileData.TileName) ? null : tiles[tileData.TileName]);
            }
        }

        tilemap.CompressBounds();

        SaveSystem.DeleteFile(Filename);
        SaveScene();
    }

    private void SaveScene()
    {
        EditorUtility.SetDirty(tilemap); //Notifies the game that there was a change to the tilemap
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

    [InspectorButton]
    private void LoadLevel()
    {
        SaveSystem.LoadLevel(this, levelName);
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(Utils.LeftMouseButton))
        // {
        //     Vector3Int pos = tilemap.WorldToCell(Utils.MouseWorldPosition);
        //     Debug.Log(pos);
        // }
    }

#endif
}
