using UnityEngine;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] private GameObject ingameUI;

    private GameObject buttonMenu;
    private GameObject saveLevelMenu;
    private GameObject loadLevelMenu;

    private void Start()
    {
        // buttonMenu = UIController.GenerateUI("Button Menu",
        //     UIController.Size.Partial, UIController.Anchor.TopLeft, 200F,
        //     TextModule.Title("Controls"),
        //     LineModule.Create());
        
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
    }

    public void DisplaySaveMenu()
    {
        saveLevelMenu.SetActive(true);
        ingameUI.SetActive(false);
    }

    public void ClearLevel()
    {
        Confirmation.Confirm("Are you sure you want to clear the level?\nThis cannot be undone")
            .OnConfirm(PlaymodeTilemapEditor.Instance.ClearTilemap);
    }

    private void SaveLevel(GameObject modules)
    {
        TextModule textModule = modules.GetModule<TextModule>(1);
        string filename = modules.GetComponentInChildren<InputFieldModule>().InputField.text;
        
        void Save()
        {
            HideMenu(modules);
            SaveSystem.SaveLevel(PlaymodeTilemapEditor.Instance, filename);
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
            SaveSystem.LoadLevel(PlaymodeTilemapEditor.Instance, filename);
            
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

        if (!PlaymodeTilemapEditor.Instance.Tilemap.IsEmpty())
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
}
