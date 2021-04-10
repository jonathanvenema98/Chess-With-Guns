using UnityEngine;

public class SpacerModule : Module
{
    public static SpacerModule Of(float height = 30F)
    {
        SpacerModule spacerModule = UIController.CreateSpacerModule;
        var rectTransform = spacerModule.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

        return spacerModule;
    }
}
