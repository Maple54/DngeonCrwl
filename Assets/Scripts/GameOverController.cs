using TMPro;
using UnityEngine;

/// <summary>
/// GameOver scene controller. Reads result and score from the persistent
/// GameManager (which survived the scene load via DontDestroyOnLoad),
/// updates the UI, and wires the back-to-menu button.
/// </summary>
public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            // Defensive — shouldn't happen if MainMenu launched properly
            resultText.text = "GAME OVER";
            finalScoreText.text = "Score: 0";
            highScoreText.text = "High Score: 0";
            return;
        }

        resultText.text = GameManager.Instance.lastRunVictory ? "VICTORY" : "DEFEAT";
        resultText.color = GameManager.Instance.lastRunVictory ? Color.yellow : Color.red;

        finalScoreText.text = $"Score: {GameManager.Instance.score}";
        highScoreText.text = $"High Score: {GameManager.Instance.highScore}";
    }

    public void OnMenuPressed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToMainMenu();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}