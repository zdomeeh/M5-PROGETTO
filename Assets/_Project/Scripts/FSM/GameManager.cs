using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxAttempts = 3;
    private int currentAttempts;

    public TextMeshProUGUI attemptsText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    public string mainMenuScene = "MainMenu";

    private void Awake()
    {
        // Singleton pattern: crea un'istanza se non esiste, altrimenti distruggi questo oggetto
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Non distruggere quando cambio scena

            currentAttempts = maxAttempts;  // Inizializza tentativi

            SceneManager.sceneLoaded += OnSceneLoaded;  // Chiama OnSceneLoaded quando carica una scena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Quando una scena viene caricata
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Trova il testo dei tentativi nella scena
        attemptsText = GameObject.Find("AttemptsText")?.GetComponent<TextMeshProUGUI>();

        // Trova il pannello Game Over
        GameObject panel = GameObject.Find("GameOverPanel");

        if (panel != null)
        {
            gameOverPanel = panel;
            gameOverText = panel.GetComponentInChildren<TextMeshProUGUI>();
            gameOverPanel.SetActive(false);  // Nascondi pannello all'inizio
        }

        UpdateUI();  // Aggiorna il testo dei tentativi
    }

    // Chiamato quando il player viene catturato
    public void PlayerCaught()
    {
        // Se un fade è già in corso, esci
        if (FadeScreen.Instance != null && FadeScreen.Instance.isFading)
            return;

        currentAttempts--;  // Riduci un tentativo
        UpdateUI();         // Aggiorna UI

        // Imposta scritta "YOU WERE CAUGHT"
        if (FadeScreen.Instance != null && FadeScreen.Instance.caughtText != null)
        {
            FadeScreen.Instance.caughtText.text = "YOU WERE CAUGHT";
        }

        // Se ci sono ancora tentativi, ricarica il livello con fade
        if (currentAttempts > 0)
        {
            FadeScreen.Instance.FadeAndExecute(ReloadLevel, true);
        }
        else // Altrimenti mostra Game Over
        {
            FadeScreen.Instance.FadeAndExecute(ShowGameOverPanel, true);
        }
    }

    // Aggiorna il testo dei tentativi nella UI
    void UpdateUI()
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Attempts: " + currentAttempts;

            // Colore rosso se rimane 1 tentativo
            if (currentAttempts == 1)
                attemptsText.color = Color.red;
            else
                attemptsText.color = Color.white;
        }
    }

    // Ricarica il livello corrente
    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Mostra il pannello Game Over
    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.enabled = true;

        StartCoroutine(WaitAndGoToMenu()); // Dopo 2 secondi, torna al menu
    }

    // Attende un po' e poi va al menu principale
    IEnumerator WaitAndGoToMenu()
    {
        yield return new WaitForSecondsRealtime(2f);
        GoToMainMenu();
    }

    // Torna al menu principale e resetta i tentativi
    public void GoToMainMenu()
    {
        currentAttempts = maxAttempts;
        SceneManager.LoadScene(mainMenuScene);
    }
}