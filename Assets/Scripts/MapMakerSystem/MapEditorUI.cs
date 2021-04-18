using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : Singleton<MapEditorUI>
{
    [SerializeField] private Sprite tileMenuSprite;
    [SerializeField] private Sprite pieceMenuSprite;
    [SerializeField] private Vector2 paintSpriteHotSpot;
    [SerializeField] private Sprite paintSprite;
    [SerializeField] private Vector2 eraseSpriteHotspot;
    [SerializeField] private Sprite eraseSprite;
    
    [SerializeField] private GameObject ingameUI;
    
    [SerializeField] private GameObject border;
    private Vector2Int borderBoardPosition;
    
    private Image currentImage;

    private GameObject saveLevelMenu;
    private GameObject loadLevelMenu;
    
    private GameObject tileMenu;
    private GameObject piecesMenu;

    private static readonly Dictionary<EditMode, Action<MapEditorUI>> EditModeInitialisers =
        new Dictionary<EditMode, Action<MapEditorUI>>
        {
            {
                EditMode.Tiles, ui =>
                {
                    ui.ToggleMenu(ui.tileMenu, EditMode.Tiles);
                    ui.SetCurrentTile(SaveSystem.Tiles.First());
                }
            },
            {
                EditMode.Pieces, ui =>
                {
                    ui.ToggleMenu(ui.piecesMenu, EditMode.Pieces);
                }
            }
        };
    
    private static readonly Dictionary<BrushMode, Action<MapEditorUI>> BrushModeInitialisers =
        new Dictionary<BrushMode, Action<MapEditorUI>>
        {
            {
                BrushMode.Paint, ui =>
                {
                    MapEditorController.CurrentBrushMode = BrushMode.Paint;
                    Cursor.SetCursor(ui.paintSprite.texture, ui.paintSpriteHotSpot, CursorMode.Auto);
                }
            },
            {
                BrushMode.Erase, ui =>
                {
                    MapEditorController.CurrentBrushMode = BrushMode.Erase;
                    Cursor.SetCursor(ui.eraseSprite.texture, ui.eraseSpriteHotspot, CursorMode.Auto);
                }
            }
        };
    
    private void Start()
    {
        UIController.GenerateUI("Button Menu", ingameUI.transform,
            Size.Partial, Anchor.TopLeft, 170F,
            TextModule.Title("Controls", 22F),
            LineModule.Create(),
            ButtonModule.Of("Save Level", _ => DisplaySaveMenu()),
            ButtonModule.Of("Load Level", _ => DisplayLoadMenu()),
            ButtonModule.Of("Clear Level", _ => ClearLevel()),
            ButtonModule.Of("Reset Camera", _ => MapEditorCameraController.ResetPosition()));

        saveLevelMenu =
            UIController.GenerateFullScreenUI("Save Level Menu",
                TextModule.Title("Save Level"),
                LineModule.Create(),
                InputFieldModule.Of("Filename"),
                SpacerModule.Of(20F),
                ConfirmationModule.Of()
                    .OnConfirm(SaveLevel)
                    .OnCancel(HideMenu),
                TextModule.Message());
        
        loadLevelMenu =
            UIController.GenerateFullScreenUI("Load Level Menu",
                TextModule.Title("Load Level"),
                LineModule.Create(),
                InputFieldModule.Of("Filename"),
                SpacerModule.Of(20F),
                ConfirmationModule.Of()
                    .OnConfirm(LoadLevel)
                    .OnCancel(HideMenu),
                TextModule.Message());

        tileMenu = UIBuilder.Of(Size.Partial)
            .HeirarchyName("Tile Menu")
            .Modules(SaveSystem.Tiles
                .Select(tile =>
                    ButtonModule.Of(tile.Sprite, _ => SetCurrentTile(tile)))
                .ToArray<Module>())
            .Build();

        piecesMenu = UIBuilder.Of(Size.Partial)
            .HeirarchyName("Piece Menu")
            .Modules(SaveSystem.Pieces
                .Select(piece => 
                    ButtonModule.Of(piece.Sprite, _ => SetCurrentPiece(piece)))
                .ToArray<Module>())
            .Build();

        var currentItemMenu = UIBuilder.Of(Size.Partial)
            .HeirarchyName("Current Item")
            .Modules(
                LineModule.Create(),
                TextModule.Title("Current Item"),
                ImageModule.Of())
            .Build();

        currentImage = currentItemMenu.GetComponentInChildren<ImageModule>().Image;

        UIBuilder.Of(Size.Partial, ingameUI.transform)
            .HeirarchyName("Edit Level Menu")
            .Anchor(Anchor.TopRight)
            .Width(200F)
            .Modules(
                TextModule.Title("Edit Level"),
                HorizontalModule.Of(30F,
                    ButtonModule.Of(paintSprite, _ => BrushModeInitialisers[BrushMode.Paint].Invoke(this)),
                    ButtonModule.Of(eraseSprite, _ => BrushModeInitialisers[BrushMode.Erase].Invoke(this))),
                LineModule.Create(),
                HorizontalModule.Of(
                    ButtonModule.Of(tileMenuSprite, _ => EditModeInitialisers[EditMode.Tiles].Invoke(this)),
                    ButtonModule.Of(pieceMenuSprite, _ => EditModeInitialisers[EditMode.Pieces].Invoke(this))),
                LineModule.Create())
            .Include(tileMenu, piecesMenu)
            .Include(currentItemMenu);
        
        EditModeInitialisers[EditMode.Tiles].Invoke(this);
        Cursor.SetCursor(paintSprite.texture, paintSpriteHotSpot, CursorMode.Auto);
    }

    private void ToggleMenu(GameObject activeMenu, EditMode newEditMode)
    {
        tileMenu.SetActive(false);
        piecesMenu.SetActive(false);
        activeMenu.SetActive(true);
        MapEditorController.CurrentEditMode = newEditMode;
    }

    private void SetCurrentTile(HeightTile tile)
    {
        MapEditorController.CurrentTile = tile;
        currentImage.sprite = tile.Sprite;
    }

    private void SetCurrentPiece(Piece piece)
    {
        MapEditorController.CurrentPiece = piece;
        currentImage.sprite = piece.Sprite;
    }

    private void DisplaySaveMenu()
    {
        saveLevelMenu.SetActive(true);
        ingameUI.SetActive(false);
    }

    private void ClearLevel()
    {
        Confirmation.Confirm("Are you sure you want to clear the level?\nThis cannot be undone")
            .OnConfirm(MapEditorController.Instance.ClearLevel);
    }

    private void SaveLevel(GameObject modules)
    {
        TextModule textModule = modules.GetModule<TextModule>(1);
        string filename = modules.GetComponentInChildren<InputFieldModule>().InputField.text;
        
        void Save()
        {
            HideMenu(modules);
            SaveSystem.SaveLevel(MapEditorController.Instance, filename);
            UIController.GeneratePopup("The level has been saved", 2);
        }
        
        if (string.IsNullOrEmpty(filename))
        {
            textModule.Text.text = "You must enter a filename";
            return;
        }

        if (SaveSystem.FileExists(filename))
        {
            HideMenu(modules);
            Confirmation.Confirm($"The level '{filename}' already exists.\nContinuing will overwrite the existing data")
                .OnConfirm(Save);
            return;
        }
        
        Save();
    }

    public void DisplayLoadMenu()
    {
        loadLevelMenu.SetActive(true);
        ingameUI.SetActive(false);
    }

    private void HideMenu(GameObject modules)
    {
        modules.SetActive(false);
        ingameUI.SetActive(true);
        UIController.ResetModules(modules);
    }

    private void LoadLevel(GameObject modules)
    {
        TextModule textModule = modules.GetModule<TextModule>(1);
        string filename = modules.GetComponentInChildren<InputFieldModule>().InputField.text;
        
        void Load()
        {
            HideMenu(modules);
            SaveSystem.LoadLevel(MapEditorController.Instance, filename);
            
            string message = SaveSystem.SuccessfullyLoaded
                ? "Level has been successfully loaded!"
                : $"There was an error while trying to load the level '{filename}'";

            UIController.GeneratePopup(message, 2);
        }
        
        if (string.IsNullOrEmpty(filename))
        {
            textModule.Text.text = "You must enter a filename";
            return;
        }

        if (!SaveSystem.FileExists(filename))
        {
            textModule.Text.text = $"The level '{filename}' does not exist.";
            return;
        }

        if (!MapEditorController.Instance.Tilemap.IsEmpty())
        {
            HideMenu(modules);
            Confirmation.Confirm(
                    "Your current tilemap isn't empty, loading this level will overwrite any unsaved changes" +
                    "\nAre you sure you want to continue?")
                .OnConfirm(Load);
            return;
        }

        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !Utils.IsMouseOverUI())
            ToggleBrushMode();

        if (Utils.IsMouseOverUI())
            return;
        
        var boardPosition = BoardController.WorldPositionToBoardPosition(Utils.MouseWorldPosition);
        if (boardPosition != borderBoardPosition)
        {
            borderBoardPosition = boardPosition;
            border.transform.position = BoardController.BoardPositionToWorldPosition(boardPosition);
        }
    }

    private void ToggleBrushMode()
    {
        var newBrushMode = MapEditorController.CurrentBrushMode == BrushMode.Paint
            ? BrushMode.Erase
            : BrushMode.Paint;
        
        BrushModeInitialisers[newBrushMode].Invoke(this);
    }
}