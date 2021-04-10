using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonModule : Module, IInjectItem<GameObject>
{
    [SerializeField] private Button button;

    public Button Button => button;
    public GameObject Item { get; set; }
    
    public static ButtonModule Of(string buttonText, Action<GameObject> callback)
    {
        ButtonModule buttonModule = UIController.CreateButtonModule;
        buttonModule.GetComponentInChildren<TMP_Text>().text = buttonText;
        buttonModule.Button.onClick.AddListener(delegate
        {
            callback.Invoke(buttonModule.Item);
        });

        return buttonModule;
    }
}
