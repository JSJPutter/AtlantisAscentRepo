using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public GameObject bossPrefab;
    public float spawnInterval = 2f;
    public float minY = -5f;
    public float maxY = 5f;
    public float spawnX = 10f;
    public float bossSpawnInterval = 500f;
    public int maxEnemies = 5; // Maximum number of enemies allowed

    private float spawnTimer = 0f;
    private float distanceTraveled = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>(); // Track active enemies

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        distanceTraveled += Time.deltaTime; // Simplified distance calculation

        if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        if (distanceTraveled >= bossSpawnInterval)
        {
            SpawnBoss();
            distanceTraveled = 0f;
        }
    }

    private void SpawnEnemy()
    {
        float cameraHalfHeight = Camera.main.orthographicSize; // Half of the camera's height
        float cameraUpperBound = Camera.main.transform.position.y + cameraHalfHeight;
        float cameraLowerBound = Camera.main.transform.position.y - cameraHalfHeight;
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        float randomY = Random.Range(cameraLowerBound, cameraUpperBound);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(spawnedEnemy); // Add to the list of active enemies

        spawnedEnemy.GetComponent<Enemy>().OnDestroyed += HandleEnemyDestroyed;
    }

    private void SpawnBoss()
    {
        float cameraHalfHeight = Camera.main.orthographicSize; // Half of the camera's height
        float cameraUpperBound = Camera.main.transform.position.y + cameraHalfHeight;
        float cameraLowerBound = Camera.main.transform.position.y - cameraHalfHeight;
        Vector3 spawnPosition = new Vector3(spawnX, 0f, 0f);
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }

    private void HandleEnemyDestroyed(GameObject enemy)
    {
        activeEnemies.Remove(enemy); 
    }
}
