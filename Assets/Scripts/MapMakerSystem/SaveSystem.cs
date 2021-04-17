using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private List<HeightTile> tiles;
    [SerializeField] private List<Piece> pieces;

    public static List<HeightTile> Tiles => Instance.tiles;
    public static List<Piece> Pieces => Instance.pieces;
    
    #region FileIO
    
    public static string Directory => "Assets\\Files\\";

    public static string GetFullFilepath(string filename)
    {
        if (!filename.EndsWith(".xml"))
            filename += ".xml";

        return Directory + filename;
    }
    
    public static bool FileExists(string filename)
    {
        return File.Exists(GetFullFilepath(filename));
    }

    public static void DeleteFile(string filename)
    {
        File.Delete(GetFullFilepath(filename));
    }

    public static void UpdateXMLNodes(string filename)
    {
        if (!FileExists(filename))
        {
            Debug.LogWarning($"The file {filename} doesn't exist.");
            return;
        }

        var nameChangeDictionary = GetNodeNameChanges();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(GetFullFilepath(filename));
        
        foreach (XmlNode child in xmlDocument.ChildNodes)
        {
            CheckForRenamedNodes(xmlDocument, child, nameChangeDictionary);
        }

        xmlDocument.Save(GetFullFilepath(filename));
        Debug.Log($"The XML Document '{filename}' has been updated.");
    }

    private static void CheckForRenamedNodes(XmlDocument xmlDocument, XmlNode root, Dictionary<string, string> nameChanges)
    {
        foreach (XmlNode child in root.ChildNodes)
        {
            CheckForRenamedNodes(xmlDocument, child, nameChanges);
        }

        if (root.Value != null && nameChanges.TryGetValue(root.Value, out string newValue))
        {
            root.Value = newValue;
        }
        if (nameChanges.TryGetValue(root.Name, out string newName))
        {
            RenameXMLNode(xmlDocument, root, newName);
        }
    }

    private static Dictionary<string, string> GetNodeNameChanges()
    {
        var nameChangeData = LoadData<ListData<NameChangeData>>("NameChangeData");
        return nameChangeData.Data.ToDictionary(nameChange => nameChange.From, nameChange => nameChange.To);
    }
    
    public static void RenameXMLNode(XmlDocument xmlDocument, XmlNode oldRoot, string newName)
    {
        XmlNode newRoot = xmlDocument.CreateElement(newName);

        foreach (XmlNode childNode in oldRoot.ChildNodes)
        {
            //Make a deep clone of all the child nodes of the old node
            newRoot.AppendChild(childNode.CloneNode(true));
        }
        
        XmlNode parent = oldRoot.ParentNode;
        parent?.AppendChild(newRoot);
        parent?.RemoveChild(oldRoot);
    }
    
    #endregion
    
    #region Levels
    
    public static void SaveLevel(ISaveLevelDriver saveLevelDriver, string levelName)
    {
        var tilemapData = new TilemapData(saveLevelDriver.Tilemap
            .GetAllTilePositions()
            .Select(position => new TileData(position, saveLevelDriver.Tilemap.GetTile(position).name))
            .ToList());

        var pieceData = new ListData<PieceData>(
            saveLevelDriver.GetPieces()
            .Select(piece => new PieceData {CellPosition = BoardController.V2ToV3(piece.BoardPosition), Name = piece.name, Team = piece.Team})
            .ToList());

        var levelData = new LevelData(tilemapData, pieceData, saveLevelDriver.Tilemap);
        SaveData(levelName, levelData);
    }

    public static void LoadLevel(ILoadLevelDriver loadLevelDriver, string levelName, Action<LevelData> preloadInitialisation = null)
    {
        if (!FileExists(levelName))
        {
            Debug.LogWarning($"The level {levelName} doesn't exist.");
            return;
        }

        var levelData = LoadData<LevelData>(levelName);
        preloadInitialisation?.Invoke(levelData);
        
        var tiles = CreateTileMappings();
        var pieces = CreatePieceMappings();
        
        //Centres the loaded board
        Vector3Int cellPositionOffset = levelData.TilemapSize / 2 + levelData.TilemapOrigin;

        loadLevelDriver.ClearLevel();
        
        foreach (var tileData in levelData.TilemapData.Tiles)
        {
            if (tiles.TryGetValue(tileData.Name, out TileBase tile))
            {
                Vector3Int cellPosition = tileData.CellPosition - cellPositionOffset;
                loadLevelDriver.SetTile(cellPosition, tile);
            }
            else TileNotFound(tileData.Name);
        }

        foreach (var pieceData in levelData.PieceData.Data)
        {
            if (pieces.TryGetValue(pieceData.Name, out Piece piece))
            {
                loadLevelDriver.SetPiece(pieceData.CellPosition - cellPositionOffset, piece);
            }
            else PieceNotFound(pieceData.Name);
        }   
    }

    private static void PieceNotFound(string pieceName)
    {
        Debug.LogWarning($"Couldn't find the piece: {pieceName}");
    }
    
    private static void TileNotFound(string tilename)
    {
        Debug.LogWarning($"Couldn't find the tile: {tilename}");
    }

    public static Dictionary<string, TileBase> CreateTileMappings()
    {
        var tiles = new Dictionary<string, TileBase>();
        if (Tiles != null)
        {
            foreach (var tile in Tiles)
            {
                tiles.Add(tile.name, tile);
            }
        }

        return tiles;
    }
    
    public static Dictionary<string, Piece> CreatePieceMappings()
    {
        var pieces = new Dictionary<string, Piece>();
        if (Pieces != null)
        {
            foreach (var piece in Pieces)
            {
                pieces.Add(piece.name, piece);
            }
        }

        return pieces;
    }

    #endregion

    #region Serializing

    public static void SaveData<T>(string filename, T data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        TextWriter writer = new StreamWriter(GetFullFilepath(filename));

        serializer.Serialize(writer, data);
        writer.Close();
    }

    public static bool SuccessfullyLoaded { get; private set; }

    public static T LoadData<T>(string filename)
    {
        bool updated = false;
        while (true) //Attempts to load the document twice by 'Updating' the XML nodes if it fails.
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            //If the XML Document has been altered with unknown nodes or is an out of date data save:
            serializer.UnknownNode += HandleUnknownNode;
            SuccessfullyLoaded = true;

            using FileStream stream = new FileStream(GetFullFilepath(filename), FileMode.Open, FileAccess.Read);
            T data = (T) serializer.Deserialize(stream);
            stream.Close();

            if (SuccessfullyLoaded) return data;

            if (!updated)
            {
                Debug.Log($"Trying to automatically update '{filename}'");
                UpdateXMLNodes(filename);

                updated = true;
                continue;
            }

            Debug.Log($"Failed to update '{filename}'");
            return data;
        }
    }

    private static void HandleUnknownNode(object sender, XmlNodeEventArgs e)
    {
        Debug.LogWarning($"Unknown Node: [{e.Name}:{e.Text}]");
        SuccessfullyLoaded = false;
    }

    #endregion
}
