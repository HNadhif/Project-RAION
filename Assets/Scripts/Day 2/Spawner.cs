using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject EnemyTripleShotPrefab;
    public GameObject EnemyYellowPrefab;
    [Header("Spawn Intervals")]
    [Tooltip("Interval untuk spawn enemy biasa (detik)")]
    public float SpawnInterval = 3f;
    [Tooltip("Interval untuk spawn enemy triple-shot setelah tercapai score threshold (detik)")]
    public float TripleSpawnInterval = 5f;
    [Tooltip("Interval untuk spawn enemy yellow setelah tercapai score threshold (detik)")]
    public float YellowSpawnInterval = 8f;

    // Timers untuk masing-masing spawner
    private float sinceLastSpawnRegular = 0f;
    private float sinceLastSpawnTriple = 0f;
    private float sinceLastSpawnYellow = 0f;
    public float spawnBoundaryY = 5f;
    public float spawnBoundaryX = 10f;

    // Spawn generic helper
    void Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.Log("Prefab enemy tidak ada, tambahkan dulu");
            return;
        }

        float RandomY = Random.Range(spawnBoundaryX, spawnBoundaryY);
        Vector3 spawnPosition = new Vector3(9, RandomY, 0);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;

        // Update all timers
        sinceLastSpawnRegular += Time.deltaTime;
        sinceLastSpawnTriple += Time.deltaTime;
        sinceLastSpawnYellow += Time.deltaTime;

        // Regular enemy spawns using SpawnInterval
        if (sinceLastSpawnRegular >= SpawnInterval)
        {
            Spawn(EnemyPrefab);
            sinceLastSpawnRegular = 0f;
        }

        // Triple-shot enemies start spawning when score threshold reached
        if (currentScore >= 2500 && EnemyTripleShotPrefab != null)
        {
            if (sinceLastSpawnTriple >= TripleSpawnInterval)
            {
                Spawn(EnemyTripleShotPrefab);
                sinceLastSpawnTriple = 0f;
            }
        }

        // Yellow enemies start spawning when score threshold reached
        if (currentScore >= 4500 && EnemyYellowPrefab != null)
        {
            if (sinceLastSpawnYellow >= YellowSpawnInterval)
            {
                Spawn(EnemyYellowPrefab);
                sinceLastSpawnYellow = 0f;
            }
        }
    }
}