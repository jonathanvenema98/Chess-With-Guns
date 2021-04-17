using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonModule : Module, IInjectItem<GameObject>
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    
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
    
    public static ButtonModule Of(Sprite sprite, Action<GameObject> callback)
    {
        ButtonModule buttonModule = UIController.CreateSpriteButtonModule;
        buttonModule.image.sprite = sprite;
        buttonModule.Button.onClick.AddListener(delegate
        {
            callback.Invoke(buttonModule.Item);
        });

        return buttonModule;
    }
}
