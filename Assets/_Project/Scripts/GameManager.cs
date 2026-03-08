using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Attempts")]
    public int maxAttempts = 3;
    private int currentAttempts;

    [Header("UI")]
    public TextMeshProUGUI attemptsText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currentAttempts = maxAttempts;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempts text
        attemptsText = GameObject.Find("AttemptsText")?.GetComponent<TextMeshProUGUI>();

        // GameOverPanel
        GameObject panel = GameObject.Find("GameOverPanel");

        if (panel != null)
        {
            gameOverPanel = panel;
            gameOverText = panel.GetComponentInChildren<TextMeshProUGUI>();

            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    public void PlayerCaught()
    {
        if (FadeScreen.Instance != null && FadeScreen.Instance.isFading)
            return;

        currentAttempts--;
        UpdateUI();

        if (FadeScreen.Instance != null && FadeScreen.Instance.caughtText != null)
        {
            FadeScreen.Instance.caughtText.text = "YOU WERE CAUGHT";
        }

        if (currentAttempts > 0)
        {
            FadeScreen.Instance.FadeAndExecute(ReloadLevel, true);
        }
        else
        {
            FadeScreen.Instance.FadeAndExecute(ShowGameOverPanel, true);
        }
    }

    void UpdateUI()
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Attempts: " + currentAttempts;

            if (currentAttempts == 1)
                attemptsText.color = Color.red;
            else
                attemptsText.color = Color.white;
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.enabled = true;

        StartCoroutine(WaitAndGoToMenu());
    }

    IEnumerator WaitAndGoToMenu()
    {
        yield return new WaitForSecondsRealtime(2f);
        GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        currentAttempts = maxAttempts;
        SceneManager.LoadScene(mainMenuScene);
    }
}