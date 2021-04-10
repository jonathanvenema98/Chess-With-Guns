using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject ingameUI;
    
    [SerializeField] private InputField filenameInputField;
    [SerializeField] private Text displayMessage;

    private GameObject loadLevelMenu;
    
    private bool needsToConfirm;

    private void Start()
    {
        loadLevelMenu =
            UIController.GenerateFullScreenUI("Load Level Menu",
                TextModule.Title("Load Level"),
                LineModule.Create(),
                InputFieldModule.Of("filename"),
                ConfirmationModule.Of()
                    .OnConfirm(LoadLevel)
                    .OnCancel(HideMenu),
                TextModule.Of());
    }

    public void DisplaySaveMenu()
    {
        saveMenu.SetActive(true);
        ingameUI.SetActive(false);
    }

    public void ClearLevel()
    {
        Confirmation.Confirm("Are you sure you want to clear the level?\nThis cannot be undone")
            .OnConfirm(PlaymodeTilemapEditor.Instance.ClearTilemap);
    }

    public void HideSaveMenu()
    {
        saveMenu.SetActive(false);
        filenameInputField.text = "";
        displayMessage.text = "";
        needsToConfirm = false;
        ingameUI.SetActive(true);
    }

    public void SaveLevel()
    {
        string filename = filenameInputField.text;

        if (string.IsNullOrEmpty(filename))
        {
            displayMessage.text = "You must enter a filename";
            return;
        }

        if (!needsToConfirm && SaveSystem.FileExists(filename))
        {
            displayMessage.text = "That file already exists. Press confirm again to override it";
            needsToConfirm = true;
            return;
        }
        
        SaveSystem.SaveLevel(PlaymodeTilemapEditor.Instance, filename);
        displayMessage.text = "Level has been saved";
        
        Utils.After(2, HideSaveMenu);
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
