using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUI : MonoBehaviour
{

    private float fadingDuration;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise(float fadingDuration)
    {
        this.fadingDuration = fadingDuration;

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float startAlpha = GetComponent<Text>().color.a;

        float rate = 1.0f / fadingDuration;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmpColor = GetComponent<Text>().color;

            // fade text
            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            // Decrement loop counter
            progress += rate * Time.deltaTime;

            yield return null;
        }

        // Remove object once completely faded
        Destroy(gameObject);
    }
}
