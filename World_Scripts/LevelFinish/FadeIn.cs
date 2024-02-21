using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private bool gameIsPausedAtStart = true;

    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        fadePanel.alpha = 1;
        float currentTime = 0;
        while (currentTime <= fadeDuration)
        {
            currentTime += Time.unscaledDeltaTime;
            fadePanel.alpha = Mathf.Lerp(1, 0, currentTime / fadeDuration);
            yield return null;
        }
        if (gameIsPausedAtStart)
        {
            Time.timeScale = 1;
            gameIsPausedAtStart = false;
        }
    }
}
