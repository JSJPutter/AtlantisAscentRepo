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
        float cameraHalfHeight = Camera.main.orthographicSize; // Half of the camera's height
        float cameraUpperBound = Camera.main.transform.position.y + cameraHalfHeight;
        float cameraLowerBound = Camera.main.transform.position.y - cameraHalfHeight;
        float randomY = Random.Range(cameraLowerBound, cameraUpperBound);
        Vector3 spawnPosition = new Vector3(Random.Range(-5,5), randomY, 0f);
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}