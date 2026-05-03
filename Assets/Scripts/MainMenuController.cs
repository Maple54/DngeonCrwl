using UnityEngine;

/// <summary>
/// MainMenu scene controller. Wires Play and Quit buttons to GameManager actions.
/// Lives only in the MainMenu scene.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public void OnPlayPressed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewRun();
        }
        else
        {
            // Fallback if GameManager doesn't exist yet (shouldn't happen)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
    }

    public void OnQuitPressed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
        else
        {
            Application.Quit();
        }
    }
}