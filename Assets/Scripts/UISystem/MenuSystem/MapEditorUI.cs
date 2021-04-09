using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] private GameObject saveMenu;
    
    [SerializeField] private InputField filenameInputField;
    [SerializeField] private Button saveLevelButton;
    [SerializeField] private Text displayMessage;

    private bool needsToConfirm;
    
    public void DisplaySaveMenu()
    {
        saveMenu.SetActive(true);
        saveLevelButton.gameObject.SetActive(false);
    }

    public void HideSaveMenu()
    {
        saveMenu.SetActive(false);
        filenameInputField.text = "";
        displayMessage.text = "";
        needsToConfirm = false;
        saveLevelButton.gameObject.SetActive(true);
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
        
        SaveSystem.SaveLevel(PlaymodeTilemapEditor.Tilemap, filename);
        displayMessage.text = "Level has been saved";
        
        Utils.After(2, HideSaveMenu);
    }
}
