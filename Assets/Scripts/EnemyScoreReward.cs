using UnityEngine;

public class EnemyScoreReward : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private bool isBoss = false;

    public bool IsBoss => isBoss;

    public void AwardScore()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        if (LevelManager.Instance != null)
        {
            if (isBoss)
            {
                LevelManager.Instance.OnBossDefeated();
            }
            else
            {
                LevelManager.Instance.OnEnemyDefeated();
            }
        }
    }
}