using System.Collections;
using TMPro;
using UnityEngine;

public class FadingUI : MonoBehaviour
{

    private float fadingDuration;
    private Color color;
    private TMP_Text text;

    public void Initialise(float fadingDuration)
    {
        this.fadingDuration = fadingDuration;
        text = GetComponent<TMP_Text>();
        
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float startAlpha = text.color.a;

        float rate = 1.0f / fadingDuration;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmpColor = text.color;

            // fade text
            text.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            // Decrement loop counter
            progress += rate * Time.deltaTime;

            yield return null;
        }

        // Remove object once completely faded
        Destroy(gameObject);
    }
}
