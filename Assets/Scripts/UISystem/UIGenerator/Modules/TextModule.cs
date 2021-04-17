using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextModule : Module, IResetable
{
    [SerializeField] protected TMP_Text text;
    private string initialMessage;
    
    public TMP_Text Text => text;

    public void Reset()
    {
        Text.text = initialMessage;
    }

    public static TextModule Title(string message, float fontSize = 25)
    {
        TextModule textModule = UIController.CreateTextModule;
        textModule.Text.text = message;
        textModule.text.fontSize = fontSize;
        textModule.initialMessage = message;

        return textModule;
    }

    public static TextModule Message(string message = "", float fontSize = 18)
    {
        TextModule textModule = UIController.CreateTextModule;
        textModule.Text.text = message;
        textModule.text.fontSize = fontSize;
        textModule.initialMessage = message;
        var fitter = textModule.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        return textModule;
    }
}
