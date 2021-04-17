using System.Collections.Generic;
using UnityEngine.Tilemaps;

public interface ISaveLevelDriver
{
    Tilemap Tilemap { get; }

    IEnumerable<Piece> GetPieces();
}
