using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public void Show()
    {
        UIController.IsUIActive = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        UIController.IsUIActive = false;
        gameObject.SetActive(false);
    }
}
