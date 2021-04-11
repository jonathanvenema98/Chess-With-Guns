using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Height Tile", menuName = "Tiles/Height Tile")]
public class HeightTile : TileBase
{
    public enum TileHeight
    {
        //Number corresponds to the height in pixels
        Full = 16,
        Half = 10
    }
    [SerializeField] private Sprite sprite;
    [SerializeField] private TileHeight height;
    
    public TileHeight Height => height;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        tileData.sprite = sprite;
    }
}
