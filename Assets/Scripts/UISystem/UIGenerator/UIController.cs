using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject fullScreenUIPrefab;
    [SerializeField] private GameObject popupUIPrefab;
    [SerializeField] private Transform uiParent;

    [SerializeField] private Confirmation confirmationPrefab;
    [SerializeField] private LineModule lineModulePrefab;
    [SerializeField] private InputFieldModule inputFieldModulePrefab;
    [SerializeField] private ConfirmationModule confirmationModulePrefab;
    [SerializeField] private TextModule textModulePrefab;
    [SerializeField] private ButtonModule buttonModulePrefab;

    public static Transform UIParent => Instance.uiParent;
    public static GameObject CreateFullScreenUI => Instantiate(Instance.fullScreenUIPrefab, UIParent);
    public static GameObject CreatePopupUI => Instantiate(Instance.popupUIPrefab, UIParent);

    public static Confirmation CreateConfirmation => Instantiate(Instance.confirmationPrefab, UIParent);
    public static InputFieldModule CreateInputFieldModule => Instantiate(Instance.inputFieldModulePrefab);
    public static ConfirmationModule CreateConfirmationModule => Instantiate(Instance.confirmationModulePrefab);
    public static TextModule CreateTextModule => Instantiate(Instance.textModulePrefab);
    public static LineModule CreateLineModule => Instantiate(Instance.lineModulePrefab);
    public static ButtonModule CreateButtonModule => Instantiate(Instance.buttonModulePrefab);

    public static GameObject GenerateFullScreenUI(string name, params Module[] modules)
    {
        var container = CreateFullScreenUI;
        container.name = name;
        container.SetActive(false);

        return GenerateUI(container, modules);
    }

    private static GameObject GenerateUI(GameObject container, params Module[] modules)
    {

        foreach (var module in modules)
        {
            module.transform.SetParent(container.transform);
            if (module is IInjectItem<GameObject> injectable)
                injectable.Item = container;
        }

        return container;
    }

    public static GameObject GeneratePopup(string message, float duration = -1)
    {
        var container = CreatePopupUI;
        container.name = "Popup";

        GenerateUI(container,
            TextModule.Of(message),
            LineModule.Create(),
            ButtonModule.Of("Close", Destroy));

        if (duration > 0)
            Utils.After(duration, () =>
            {
                if (container != null)
                    Destroy(container);
            });

        return container;
    }

    public static void ResetModules(GameObject container)
    {
        var modules = container.GetComponentsInChildren<Module>();
        foreach (var module in modules)
        {
            if (module is IResetable resetable)
            {
                resetable.Reset();
            }
        }
    }

}
