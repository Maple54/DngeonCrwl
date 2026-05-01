using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Run state")]
    public int score;
    public int currentLevel = 1;
    public int highScore;

    [Header("Last run result")]
    public bool lastRunVictory;

    private const string HighScoreKey = "DungeonHighScore";

    private void Awake()
    {
        // Singleton — only one GameManager exists across all scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score: {score}");
    }

    public void StartNewRun()
    {
        score = 0;
        currentLevel = 1;
        lastRunVictory = false;
        SceneManager.LoadScene("Level1");
    }

    public void EndRun(bool victory)
    {
        lastRunVictory = victory;
        TrySaveHighScore();
        SceneManager.LoadScene("GameOver");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit"); // Debug.Log appears in editor since Application.Quit doesn't work there
    }

    private void TrySaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }
}   