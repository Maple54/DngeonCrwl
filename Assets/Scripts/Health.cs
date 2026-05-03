using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private UnityEvent onDeath;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        onDeath?.Invoke();

        // If the player died, end the run as a loss
        if (CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndRun(false);
            }
            // Don't destroy the player here — GameManager will load a new scene
            return;
        }

        Destroy(gameObject);
    }
}
