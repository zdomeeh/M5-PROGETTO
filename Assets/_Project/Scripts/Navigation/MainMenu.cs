using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Funzione collegata al pulsante Start
    public void StartGame()
    {
        SceneManager.LoadScene("Lvl1");
    }

    // Funzione collegata al pulsante Exit
    public void ExitGame()
    {
        // In Build chiuderà il gioco
        Debug.Log("Exit Game clicked! Applicazione chiusa (solo in build).");
        Application.Quit();
    }
}
