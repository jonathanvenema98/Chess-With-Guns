using UnityEngine;
using UnityEngine.UI;

public class ImageModule : Module
{
    [SerializeField] private Image image;

    public Image Image => image;
    
    public static ImageModule Of(Sprite sprite)
    {
        ImageModule imageModule = UIController.CreateImageModule;
        imageModule.image.sprite = sprite;
        
        return imageModule;
    }
    
    public static ImageModule Of()
    {
        ImageModule imageModule = UIController.CreateImageModule;
        return imageModule;
    }
}
