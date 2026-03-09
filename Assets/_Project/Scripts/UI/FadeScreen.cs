using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance;

    public Image fadeImage;                  // overlay nero
    public TextMeshProUGUI caughtText;       // scritta "YOU WERE CAUGHT"

    public float fadeDuration = 0.7f;
    public float displayTime = 1f;

    // Indica se il fade è in corso
    public bool isFading { get; private set; }

    private void Awake()
    {
        // Singleton pattern: se non c'è istanza la creo, altrimenti distruggo questo oggetto
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Prepara il fade overlay: attivo ma trasparente
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        // Nasconde la scritta inizialmente
        if (caughtText != null)
            caughtText.enabled = false;
    }

    // Funzione pubblica per avviare il fade e poi eseguire un'azione
    public void FadeAndExecute(System.Action action, bool showCaughtText)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        StartCoroutine(FadeRoutine(action, showCaughtText));
    }

    // Coroutine che gestisce il fade in/out
    private IEnumerator FadeRoutine(System.Action action, bool showCaughtText)
    {
        if (isFading) yield break;  // se un fade è già in corso, esce
        isFading = true;

        // Blocca player e nemici disabilitando i loro script
        MonoBehaviour[] gameplayScripts = FindObjectsOfType<MonoBehaviour>();
        foreach (var s in gameplayScripts)
            if (s is PlayerController || s is EnemyController)
                s.enabled = false;

        // Mostra overlay e rendilo cliccabile (blocca input)
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true;
        }

        // Mostra scritta "YOU WERE CAUGHT" se richiesto
        if (caughtText != null && showCaughtText)
            caughtText.enabled = true;

        // FADE IN: aumenta gradualmente l'alpha da 0 a 1
        float t = 0f;
        Color fadeColor = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;                // usa tempo reale, non influenzato da pause
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeColor.a = alpha;
            if (fadeImage != null)
                fadeImage.color = fadeColor;
            yield return null;
        }

        // Mantieni scritta visibile per "displayTime"
        yield return new WaitForSecondsRealtime(displayTime);

        // FADE OUT: diminuisce gradualmente l'alpha da 1 a 0
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

        // Nasconde overlay e scritta
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
            fadeColor.a = 0f;
            fadeImage.color = fadeColor;
        }

        if (caughtText != null)
            caughtText.enabled = false;

        // Esegue l'azione passata come parametro
        action?.Invoke();

        isFading = false;  // il fade è terminato
    }
}