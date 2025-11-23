using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpawnInterval = 3f;
    private float SinceLastSpawn = 0; // Counter untuk spawner
    public float spawnBoundaryY = 5f;
    public float spawnBoundaryX = 10f;

    void SpawnEnemy()
    {
        if (EnemyPrefab == null)
        {
            Debug.Log("Prefab enemy tidak ada, tambahkan dulu");
            return;
        }

        float RandomY = Random.Range(spawnBoundaryX, spawnBoundaryY);

        Vector3 spawnPosition = new Vector3(9 , RandomY, 0);

        GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
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