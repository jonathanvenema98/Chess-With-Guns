using TMPro;
using UnityEngine;

public class FadingUIManager : Singleton<FadingUIManager>
{
    public GameObject fadingTextPrefab;

    public RectTransform canvasTransform;

    public float fadingDuration;
    public Color color;
    public bool crit;

    public float speed;
    public Vector3 direction;

    public void CreateFadingText(Vector2Int boardPosition, string messageText, Color color, float fadingDur = 1.0f)
    {
        GameObject fto = Instantiate(fadingTextPrefab, canvasTransform);

        fto.transform.position = BoardController.BoardPositionToWorldPosition(boardPosition);
        fto.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        fto.GetComponent<FadingUI>().Initialise(speed, direction, fadingDuration = fadingDur);
        var text = fto.GetComponent<TMP_Text>();
        
        text.text = messageText;
        text.color = color;

    }
}

