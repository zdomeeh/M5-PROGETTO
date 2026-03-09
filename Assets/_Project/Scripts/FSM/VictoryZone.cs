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
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (victoryPanel != null)
                victoryPanel.SetActive(true);

            StartCoroutine(ReturnToMainMenu());
        }
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(delayBeforeMainMenu);
        SceneManager.LoadScene("MainMenu"); // sostituisci con il nome della tua scena menu
    }
}