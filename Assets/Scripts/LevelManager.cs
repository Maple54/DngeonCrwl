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
        EnemyChase[] enemies = FindObjectsByType<EnemyChase>(FindObjectsSortMode.None);
        aliveEnemyCount = enemies.Length;

        if (bossDoor != null) bossDoor.SetActive(true);

        Debug.Log($"Level started with {aliveEnemyCount} enemies.");
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

    private void UnlockBoss()
    {
        bossUnlocked = true;
        if (bossDoor != null) bossDoor.SetActive(false);
        Debug.Log("Boss room unlocked!");
    }
}