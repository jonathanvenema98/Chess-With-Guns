using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private Grid gridPalette;
    
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
    
    public static void SaveLevel(Tilemap tilemap, string levelName)
    {
        Debug.Log(tilemap
            .GetAllTilePositions().Count);
        
        var tilemapData = new TilemapData(tilemap
            .GetAllTilePositions()
            .Select(position => new TileData(position, tilemap.GetTile(position).name))
            .ToList());

        var levelData = new LevelData(tilemapData);
        SaveData(levelName, levelData);
    }
    
    public static Dictionary<string, TileBase> GetTilePalette()
    {
        var tiles = new Dictionary<string, TileBase>();
        if (Instance.gridPalette != null)
        {
            Tilemap tilemapPalette = Instance.gridPalette.GetComponentInChildren<Tilemap>();
            if (tilemapPalette != null)
            {
                tilemapPalette
                    .GetAllTilePositions()
                    .Select(position => tilemapPalette.GetTile(position))
                    .ForEach(tile => tiles.Add(tile.name, tile));
            }
        }

        return tiles;
    }

    #region Serializing

    public static void SaveData<T>(string filename, T data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        TextWriter writer = new StreamWriter(GetFullFilepath(filename));

        serializer.Serialize(writer, data);
        writer.Close();
    }

    public static T LoadData<T>(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //If the XML Document has been altered with unknown nodes or attributes handle them:
        serializer.UnknownNode += HandleUnknownNode;
        serializer.UnknownAttribute += HandleUnknownAttribute;

        using FileStream stream = new FileStream(GetFullFilepath(filename), FileMode.Open, FileAccess.Read);
        return (T) serializer.Deserialize(stream);
    }
    
    private static void HandleUnknownNode(object sender, XmlNodeEventArgs e)
    {
        Debug.LogWarning($"Unknown Node: {e.Name}\t{e.Text}");
    }

    private static void HandleUnknownAttribute (object sender, XmlAttributeEventArgs e)
    {
        Debug.LogWarning($"Unknown Attribute: {e.Attr.Name}='{e.Attr.Value}'");
    }
    
    #endregion
}
