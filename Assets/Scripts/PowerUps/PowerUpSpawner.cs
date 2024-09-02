using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUpPrefabs;
    public float spawnInterval = 10f;
    public float minY = -5f;
    public float maxY = 5f;
    public float spawnX = 10f;

    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnPowerUp();
            spawnTimer = 0f;
        }
    }

    private void SpawnPowerUp()
    {
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}