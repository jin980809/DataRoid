using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    [Space(10)]
    [Header("Fade")]
    public Image fadeImage;
    private bool isFadingout;
    public float fadeSpeed;
    public float duration;

    private void Start()
    {
        //FadeInOut();
    }


    public void FadeInOut()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn( ));
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(duration + fadeSpeed);

        float alpha = 1.0f;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    { 
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
