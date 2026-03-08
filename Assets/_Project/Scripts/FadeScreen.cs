using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance;

    [Header("UI Elements")]
    public Image fadeImage;                  // overlay nero
    public TextMeshProUGUI caughtText;       // scritta "YOU WERE CAUGHT"

    [Header("Fade Settings")]
    public float fadeDuration = 0.7f;
    public float displayTime = 1f;

    public bool isFading { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        if (caughtText != null)
            caughtText.enabled = false;
    }

    public void FadeAndExecute(System.Action action, bool showCaughtText)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        StartCoroutine(FadeRoutine(action, showCaughtText));
    }

    private IEnumerator FadeRoutine(System.Action action, bool showCaughtText)
    {
        if (isFading) yield break;
        isFading = true;

        // Blocca player e nemici
        MonoBehaviour[] gameplayScripts = FindObjectsOfType<MonoBehaviour>();
        foreach (var s in gameplayScripts)
            if (s is PlayerController || s is EnemyController)
                s.enabled = false;

        // Fade in
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true;
        }

        if (caughtText != null && showCaughtText)
            caughtText.enabled = true;

        float t = 0f;
        Color fadeColor = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeColor.a = alpha;
            if (fadeImage != null)
                fadeImage.color = fadeColor;
            yield return null;
        }

        // Mantieni scritta visibile
        yield return new WaitForSecondsRealtime(displayTime);

        // Fade out
        t = 0f;
        float startAlpha = fadeImage.color.a;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            fadeColor.a = alpha;
            if (fadeImage != null)
                fadeImage.color = fadeColor;
            yield return null;
        }

        // Nascondi overlay e scritta
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
            fadeColor.a = 0f;
            fadeImage.color = fadeColor;
        }

        if (caughtText != null)
            caughtText.enabled = false;

        // Esegui azione passata
        action?.Invoke();

        isFading = false;
    }
}