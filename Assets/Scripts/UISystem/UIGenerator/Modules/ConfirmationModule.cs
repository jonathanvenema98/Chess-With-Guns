using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationModule : Module, IInjectItem<GameObject>
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    
    public GameObject Item { get; set; }

    public static ConfirmationModule Of()
    {
        var confirmation = UIController.CreateConfirmationModule;
        return confirmation;
    }

    public ConfirmationModule  OnConfirm(Action<GameObject> callback)
    {
        confirmButton.onClick.AddListener(delegate
        {
            callback?.Invoke(Item);
        });
        return this;
    }

    public ConfirmationModule  OnCancel(Action<GameObject> callback)
    {
        cancelButton.onClick.AddListener(delegate
        {
            callback?.Invoke(Item);
        });
        return this;
    }
}