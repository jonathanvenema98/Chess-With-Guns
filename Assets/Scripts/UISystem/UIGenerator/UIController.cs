using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject fullScreenUIPrefab;
    [SerializeField] private GameObject popupUIPrefab;
    [SerializeField] private Transform uiParent;

    [SerializeField] private LineModule lineModulePrefab;
    [SerializeField] private InputFieldModule inputFieldModulePrefab;
    [SerializeField] private ConfirmationModule confirmationModulePrefab;
    [SerializeField] private TextModule textModulePrefab;
    [SerializeField] private ButtonModule buttonModulePrefab;
    [SerializeField] private ButtonModule spriteButtonModulePrefab;
    [SerializeField] private SpacerModule spacerModulePrefab;
    [SerializeField] private HorizontalModule horizontalModulePrefab;
    [SerializeField] private ImageModule imageModulePrefab;
    
    public static Transform UIParent => Instance.uiParent;
    public static GameObject FullScreenUIPrefab => Instance.fullScreenUIPrefab;
    public static GameObject PopupUIPrefab => Instance.popupUIPrefab;
    public static GameObject CreateFullScreenUI => Instantiate(Instance.fullScreenUIPrefab, UIParent);
    public static GameObject CreatePopupUI => Instantiate(Instance.popupUIPrefab, UIParent);

    public static InputFieldModule CreateInputFieldModule => Instantiate(Instance.inputFieldModulePrefab);
    public static ConfirmationModule CreateConfirmationModule => Instantiate(Instance.confirmationModulePrefab);
    public static TextModule CreateTextModule => Instantiate(Instance.textModulePrefab);
    public static LineModule CreateLineModule => Instantiate(Instance.lineModulePrefab);
    public static ButtonModule CreateButtonModule => Instantiate(Instance.buttonModulePrefab);
    public static ButtonModule CreateSpriteButtonModule => Instantiate(Instance.spriteButtonModulePrefab);
    public static SpacerModule CreateSpacerModule => Instantiate(Instance.spacerModulePrefab);
    public static HorizontalModule CreateHorizontalModule => Instantiate(Instance.horizontalModulePrefab);
    public static ImageModule CreateImageModule => Instantiate(Instance.imageModulePrefab);
    
    public static bool IsUIActive { get; set; }

    public static readonly Dictionary<Anchor, Action<RectTransform>> GetAnchorSetter =
        new Dictionary<Anchor, Action<RectTransform>>
        {
            {Anchor.TopLeft, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
            }},
            {Anchor.TopCentre, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0.5F, 1);
                rectTransform.anchorMax = new Vector2(0.5F, 1);
                rectTransform.pivot = new Vector2(0.5F, 1);
            }},
            {Anchor.TopRight, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(1, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 1);
            }},
            {Anchor.CentreLeft, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0, 0.5F);
                rectTransform.anchorMax = new Vector2(0, 0.5F);
                rectTransform.pivot = new Vector2(0, 0.5F);
            }},
            {Anchor.Centre, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0.5F, 0.5F);
                rectTransform.anchorMax = new Vector2(0.5F, 0.5F);
                rectTransform.pivot = new Vector2(0.5F, 0.5F);
            }},
            {Anchor.CentreRight, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(1, 0.5F);
                rectTransform.anchorMax = new Vector2(1, 0.5F);
                rectTransform.pivot = new Vector2(1, 0.5F);
            }},
            {Anchor.BottomLeft, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0, 0);
            }},
            {Anchor.BottomCentre, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0.5F, 0);
                rectTransform.anchorMax = new Vector2(0.5F, 0);
                rectTransform.pivot = new Vector2(0.5F, 0);
            }},
            {Anchor.BottomRight, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(1, 0);
            }},
            {Anchor.Fill, rectTransform =>
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5F, 0.5F);
            }}
        };

    public static GameObject GetSizePrefab(Size size)
    {
        return size switch
        {
            Size.Fullscreen => FullScreenUIPrefab,
            Size.Partial => PopupUIPrefab,
            _ => PopupUIPrefab
        };
    }
    
    public static GameObject GenerateUI(string name, Transform parent, Size size, Anchor anchor, float width, params Module[] modules)
    {
        var containerPrefab = size == Size.Fullscreen ? Instance.fullScreenUIPrefab : Instance.popupUIPrefab;
        var container = Instantiate(containerPrefab, parent);
        container.name = name;
        var rect = container.GetComponent<RectTransform>();
        GetAnchorSetter[anchor].Invoke(rect);
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        
        return GenerateUI(container, modules);
    }

    public static GameObject GenerateFullScreenUI(string name, params Module[] modules)
    {
        var container = CreateFullScreenUI;
        container.name = name;
        container.SetActive(false);

        return GenerateUI(container, modules);
    }

    public static GameObject GenerateUI(GameObject container, params Module[] modules)
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
            TextModule.Message(message),
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
    
    public static GameObject GeneratePopup(string name, params Module[] modules)
    {
        var container = CreatePopupUI;
        container.name = name;

        return GenerateUI(container, modules);
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

    public static void DeleteModules(GameObject modules)
    {
        Destroy(modules);
    }
}

public enum Size
{
    Fullscreen, Partial
}

public enum Anchor
{
    TopLeft, TopCentre, TopRight, CentreLeft, Centre, CentreRight, BottomLeft, BottomCentre, BottomRight, Fill
}
