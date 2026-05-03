using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level setup")]
    [SerializeField] private GameObject bossDoor;

    private int aliveEnemyCount;
    private bool bossUnlocked;

    public bool BossUnlocked => bossUnlocked;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Count only non-boss enemies — the boss is tracked separately
        EnemyScoreReward[] rewards = FindObjectsByType<EnemyScoreReward>(FindObjectsSortMode.None);
        aliveEnemyCount = 0;

        foreach (EnemyScoreReward reward in rewards)
        {
            if (!reward.IsBoss) aliveEnemyCount++;
        }

        if (bossDoor != null) bossDoor.SetActive(true);

        Debug.Log($"Level started with {aliveEnemyCount} regular enemies (boss tracked separately).");
    }

    public void OnEnemyDefeated()
    {
        aliveEnemyCount--;
        Debug.Log($"Enemy defeated. Remaining: {aliveEnemyCount}");

        if (aliveEnemyCount <= 0 && !bossUnlocked)
        {
            UnlockBoss();
        }
    }

    public void OnBossDefeated()
    {
        Debug.Log("Boss defeated! Victory!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndRun(true);
        }
    }

    private void UnlockBoss()
    {
        bossUnlocked = true;
        if (bossDoor != null) bossDoor.SetActive(false);
        Debug.Log("Boss room unlocked!");
    }
}