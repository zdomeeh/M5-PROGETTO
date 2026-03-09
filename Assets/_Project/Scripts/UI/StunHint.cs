using System.Collections;
using UnityEngine;

public class StunHint : MonoBehaviour
{
    public GameObject hintPanel; // Il panel con il messaggio "Premi R per stunnare i nemici"

    public float displayTime = 5f; // tempo in secondi prima di nascondere l'hint

    private void Start()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);            // Mostra l'hint all'inizio
            StartCoroutine(HideHintAfterTime());  // Avvia la coroutine per nascondere l'hint
        }
    }

    // Coroutine che aspetta displayTime secondi prima di nascondere l'hint
    private IEnumerator HideHintAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        if (hintPanel != null)
            hintPanel.SetActive(false);         // Nasconde il pannello
    }
}