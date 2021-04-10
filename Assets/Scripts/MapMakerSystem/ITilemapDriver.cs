using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITilemapDriver
{
    Tilemap Tilemap { get; }

    void ClearTilemap();

    void SetTile(Vector3Int position, TileBase tile);
}
