using TMPro;
using UnityEngine;

public class FadingUIManager : Singleton<FadingUIManager>
{
    public GameObject fadingTextPrefab;

    public RectTransform canvasTransform;

    public float fadingDuration;
    public Color color;
    public bool crit;

    public void CreateFadingText(Vector2Int boardPosition, string messageText, Color color)
    {
        GameObject fto = Instantiate(fadingTextPrefab, canvasTransform);

        fto.transform.position = BoardController.BoardPositionToWorldPosition(boardPosition);
        fto.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        fto.GetComponent<FadingUI>().Initialise(fadingDuration);
        var text = fto.GetComponent<TMP_Text>();
        
        text.text = messageText;
        text.color = color;

    }
}

