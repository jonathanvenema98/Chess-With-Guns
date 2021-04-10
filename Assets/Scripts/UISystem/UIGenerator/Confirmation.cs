using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private Action onConfirm;
    private Action onCancel;
    
    private void Start()
    {
        confirmButton.onClick.AddListener(delegate
        {
            onConfirm?.Invoke();
            DeleteConfirmation();
        });
        
        cancelButton.onClick.AddListener(delegate
        {
            onCancel?.Invoke();
            DeleteConfirmation();
        });
    }

    public static Confirmation Confirm(string message)
    {
        var confirmation = UIController.CreateConfirmation;
        confirmation.message.text = message;

        return confirmation;
    }

    public Confirmation OnConfirm(Action callback)
    {
        onConfirm = callback;
        return this;
    }

    public Confirmation OnCancel(Action callback)
    {
        onCancel = callback;
        return this;
    }

    public void DeleteConfirmation()
    {
        Destroy(gameObject);
    }
}
