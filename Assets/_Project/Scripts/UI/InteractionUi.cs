using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    // Mostra l'hint
    public void ShowHint()
    {
        if (hint != null)          // Controlla che l'hint sia collegato
            hint.SetActive(true);  // Attiva l'oggetto nella scena
    }

    // Nasconde l'hint
    public void HideHint()
    {
        if (hint != null)          // Controlla che l'hint sia collegato
            hint.SetActive(false); // Disattiva l'oggetto nella scena
    }
}