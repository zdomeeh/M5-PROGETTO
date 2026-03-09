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
        Debug.Log("Exit Game clicked! Applicazione chiusa.");
        Application.Quit();
    }
}
