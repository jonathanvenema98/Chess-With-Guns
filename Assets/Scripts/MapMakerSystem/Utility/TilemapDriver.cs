using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDriver : ITilemapDriver
{
    public Tilemap Tilemap { get; }

    public TilemapDriver(Tilemap tilemap)
    {
        Tilemap = tilemap;
    }
    
    public void ClearTilemap()
    {
        Tilemap.ClearAllTiles();
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
        Tilemap.SetTile(position, tile);
    }

    public static ITilemapDriver Of(Tilemap tilemap) => new TilemapDriver(tilemap);
}
