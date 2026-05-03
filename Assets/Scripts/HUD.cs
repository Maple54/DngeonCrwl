using TMPro;
using UnityEngine;

/// <summary>
/// HUD acts as the View in the MVC pattern (Lecture 3) —
/// reads from GameManager (model) and Health (model) and updates
/// the on-screen text. The HUD never modifies game state itself.
/// </summary>
public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Health playerHealth;

    private void Update()
    {
        if (playerHealth != null)
        {
            healthText.text = $"HP: {playerHealth.CurrentHealth}";
        }
        else
        {
            // Player has been destroyed — show 0
            healthText.text = "HP: 0";
        }

        if (GameManager.Instance != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.score}";
        }
    }
}