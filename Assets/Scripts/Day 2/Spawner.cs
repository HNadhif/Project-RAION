using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject EnemyTripleShotPrefab;
    public GameObject EnemyYellowPrefab;
    public float SpawnInterval = 3f;
    private float SinceLastSpawn = 0; // Counter untuk spawner
    public float spawnBoundaryY = 5f;
    public float spawnBoundaryX = 10f;

    void SpawnEnemy()
    {
        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;
        GameObject prefabToSpawn = EnemyPrefab;

        // Tentukan enemy mana yang di-spawn berdasarkan score
        // Enemy biasa selalu spawn, ditambah dengan tipe lain sesuai score
        if (currentScore >= 4500 && EnemyYellowPrefab != null)
        {
            // Spawn random antara regular, TripleShot, atau Yellow
            int randomType = Random.Range(0, 3);
            if (randomType == 0)
                prefabToSpawn = EnemyPrefab;
            else if (randomType == 1)
                prefabToSpawn = EnemyTripleShotPrefab;
            else
                prefabToSpawn = EnemyYellowPrefab;
        }
        else if (currentScore >= 2500 && EnemyTripleShotPrefab != null)
        {
            // Spawn random antara regular atau TripleShot
            int randomType = Random.Range(0, 2);
            if (randomType == 0)
                prefabToSpawn = EnemyPrefab;
            else
                prefabToSpawn = EnemyTripleShotPrefab;
        }

        if (prefabToSpawn == null)
        {
            Debug.Log("Prefab enemy tidak ada, tambahkan dulu");
            return;
        }

        float RandomY = Random.Range(spawnBoundaryX, spawnBoundaryY);

        Vector3 spawnPosition = new Vector3(9 , RandomY, 0);

        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        SinceLastSpawn += Time.deltaTime;
        if (SinceLastSpawn >= SpawnInterval)
        {
            SpawnEnemy();
            SinceLastSpawn = 0;
        }
    }
}