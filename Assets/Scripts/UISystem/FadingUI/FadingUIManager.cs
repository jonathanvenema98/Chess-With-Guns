using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUIManager : MonoBehaviour
{

    private static FadingUIManager instance;

    public GameObject fadingTextPrefab;

    public RectTransform canvasTransform;

    public float fadingDuration;
    public Color color;
    public bool crit;

    public static FadingUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<FadingUIManager>();
            }
            return instance;
        }
    }

    public void CreateFadingText(Vector2Int position, string messageText, Color color)
    {
        GameObject fto = (GameObject)Instantiate(fadingTextPrefab, BoardController.BoardPositionToWorldPosition(position), Quaternion.identity);

        fto.transform.SetParent(canvasTransform);
        fto.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        fto.GetComponent<FadingUI>().Initialise(fadingDuration);
        fto.GetComponent<Text>().text = messageText;
        fto.GetComponent<Text>().color = color;

    }
}

/*    Example function call
if(Input.GetKeyDown(KeyCode.Space))
{
    FadingUIManager.Instance.CreateFadingText(vector2position, "text message", Color.red);
}
*/
