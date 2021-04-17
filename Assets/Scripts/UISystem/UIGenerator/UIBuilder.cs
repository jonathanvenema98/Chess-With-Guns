using UnityEngine;

public class UIBuilder
{
    protected readonly GameObject container;
    
    protected UIBuilder(Size size, Transform parent = null)
    {
        if (parent == null)
            parent = UIController.UIParent;

        var prefab = UIController.GetSizePrefab(size);
        container = Object.Instantiate(prefab, parent);
    }

    public static UIBuilder Of(Size size, Transform parent = null)
    {
        return new UIBuilder(size, parent);
    }

    public UIBuilder HeirarchyName(string name)
    {
        container.name = name;
        return this;
    }

    public UIBuilder Anchor(Anchor anchor)
    {
        var rect = container.GetComponent<RectTransform>();
        UIController.GetAnchorSetter[anchor].Invoke(rect);
        return this;
    }

    public UIBuilder Width(float width)
    {
        var rect = container.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        return this;
    }

    public UIBuilder Modules(params Module[] modules)
    {
        UIController.GenerateUI(container, modules);
        return this;
    }

    public UIBuilder Include(params GameObject[] modules)
    {
        foreach (var module in modules)
        {
            module.transform.SetParent(container.transform);
        }

        return this;
    }

    public GameObject Build()
    {
        return container;
    }
}
