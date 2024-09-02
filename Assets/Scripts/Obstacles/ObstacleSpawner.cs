using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 2f;
    public float minSize = 0.5f;
    public float maxSize = 2f;

    private float timeSinceLastSpawn;
    private float screenWidth;
    private float screenHeight;

    private void Start()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        screenHeight = Camera.main.orthographicSize * 2;
        timeSinceLastSpawn = 0f;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-screenWidth / 2, screenWidth / 2),
            screenHeight / 2 + 1, // Spawn just above the visible screen
            0
        );

        GameObject prefabToSpawn;
        if (Random.value > 0.3f)  // 70% chance for obstacle, 30% for enemy
        {
            prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        }
        else
        {
            prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }

        GameObject obstacle = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Randomize size
        float randomSize = Random.Range(minSize, maxSize);
        obstacle.transform.localScale = new Vector3(randomSize, randomSize, 1);

        // Add downward movement
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        // if (rb == null) {
        //     rb = obstacle.AddComponent<Rigidbody2D>();
        // }
        rb.gravityScale = 0;
        // rb.velocity = Vector2.down * GameManager.Instance.GetCurrentScrollSpeed();
        rb.velocity = Vector2.down * 10;
    }
}