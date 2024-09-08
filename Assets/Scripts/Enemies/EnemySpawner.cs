using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner: MonoBehaviour
{
    public GameObject player;
    public List<GameObject> enemyPrefabs;
    public GameObject bossPrefab;
    public float spawnInterval = 2f;
    public float minY = -5f;
    public float maxY = 5f;
    public float spawnX = 10f;
    public float bossSpawnInterval = 500f;

    private float spawnTimer = 0f;
    private float distanceTraveled = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        distanceTraveled += Time.deltaTime; // Simplified distance calculation

        if (spawnTimer >= spawnInterval)
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
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, player.transform.position.y + randomY, 0f);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnBoss()
    {
        Vector3 spawnPosition = new Vector3(spawnX, 0f, 0f);
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }
}