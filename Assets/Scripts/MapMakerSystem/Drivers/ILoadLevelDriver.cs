using UnityEngine;
using UnityEngine.Tilemaps;

public interface ILoadLevelDriver
{
    void ClearLevel();

    void SetTile(Vector3Int cellPosition, TileBase tile);

    void SetPiece(Vector3Int cellPosition, Piece piece);
}
