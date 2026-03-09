using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    public GameObject victoryPanel; // Il panel da mostrare

    public float delayBeforeMainMenu = 3f; // tempo prima di tornare al menu

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;          // Se già attivato, non fare nulla

        if (other.CompareTag("Player")) // Controlla se il player entra nella zona
        {
            triggered = true;

            if (victoryPanel != null)
                victoryPanel.SetActive(true); // Mostra il pannello di vittoria

            StartCoroutine(ReturnToMainMenu()); // Avvia coroutine per tornare al menu
        }
    }

    // Coroutine che aspetta un po' e poi carica il menu principale
    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(delayBeforeMainMenu);
        SceneManager.LoadScene("MainMenu"); // Sostituisci con il nome della tua scena menu
    }
}