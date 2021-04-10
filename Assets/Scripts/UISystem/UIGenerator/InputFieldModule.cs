using TMPro;
using UnityEngine;

public class InputFieldModule : Module, IResetable
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_InputField inputField;

    public TMP_Text Label => label;
    public TMP_InputField InputField => inputField;

    public void Reset()
    {
        InputField.text = "";
    }

    public static InputFieldModule Of(string label)
    {
        return Of(label, $"Enter {label.ToLower()}...");
    }
    
    public static InputFieldModule Of(string label, string placeholder)
    {
        InputFieldModule inputFieldModule = UIController.CreateInputFieldModule;
        inputFieldModule.label.text = label;
        inputFieldModule.inputField.placeholder.GetComponent<TMP_Text>().text = placeholder;

        return inputFieldModule;
    }
}
