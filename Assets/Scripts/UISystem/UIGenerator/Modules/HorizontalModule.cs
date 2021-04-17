using UnityEngine;

public class HorizontalModule : Module
{
    public static HorizontalModule Of(params Module[] modules)
    {
        HorizontalModule horizontalModule = UIController.CreateHorizontalModule;
        UIController.GenerateUI(horizontalModule.gameObject, modules);

        return horizontalModule;
    }
    
    public static HorizontalModule Of(float height, params Module[] modules)
    {
        HorizontalModule horizontalModule = UIController.CreateHorizontalModule;
        UIController.GenerateUI(horizontalModule.gameObject, modules);
        var rect = horizontalModule.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        
        return horizontalModule;
    }
}
