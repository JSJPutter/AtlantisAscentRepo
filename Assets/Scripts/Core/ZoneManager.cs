using UnityEngine;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    public List<ZoneData> zones;
    public float spawnAreaWidth = 10f;
    public float despawnHeight = -10f;
    public float spawnAheadDistance = 20f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private ZoneData currentZone;
    private float lastSpawnHeight;
    private const float spawnInterval = 5f;

    private Transform player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (backgroundRenderer == null)
            backgroundRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentZone = zones[0];
        lastSpawnHeight = currentZone.startHeight;
        UpdateBackground();
    }

    private void Update()
    {
        UpdatePlayerHeight(player.position.y);
    }

    public void UpdatePlayerHeight(float height)
    {
        CheckZoneTransition(height);
        ManageZoneSpawning(height);
    }

    private void CheckZoneTransition(float height)
    {
        foreach (ZoneData zone in zones)
        {
            if (height >= zone.startHeight && height < zone.endHeight)
            {
                if (zone != currentZone)
                {
                    TransitionToNewZone(zone);
                }
                break;
            }
        }
    }

    private void TransitionToNewZone(ZoneData newZone)
    {
        currentZone = newZone;
        RenderSettings.ambientLight = currentZone.ambientLight;
        UpdateBackground();
        // Trigger any zone transition events or effects
        GameManager.Instance.TriggerEvent("ZoneChanged", currentZone);
    }

    private void UpdateBackground()
    {
        if (backgroundRenderer != null && currentZone.backgroundSprite != null)
        {
            backgroundRenderer.sprite = currentZone.backgroundSprite;
            // backgroundRenderer.color = currentZone.backgroundColor;

            // if (currentZone.tileBackgroundVertically)
            // {
                backgroundRenderer.drawMode = SpriteDrawMode.Tiled;
                backgroundRenderer.size = new Vector2(spawnAreaWidth, currentZone.endHeight - currentZone.startHeight);
            // }
            // else
            // {
                // backgroundRenderer.drawMode = SpriteDrawMode.Simple;
            // }

            // Position the background
            backgroundRenderer.transform.position = new Vector3(0, (currentZone.startHeight + currentZone.endHeight) / 2, 10);
        }
    }

    private void ManageZoneSpawning(float height)
    {
        if (height > lastSpawnHeight + spawnInterval)
        {
            SpawnZoneElements();
            lastSpawnHeight = height;
        }
    }

    private void SpawnZoneElements()
    {
        SpawnObstacles();
        SpawnEnemies();
        SpawnArtifacts();
    }

    private void SpawnObstacles()
    {
        if (currentZone.obstaclePrefabs.Count > 0)
        {
            GameObject obstaclePrefab = currentZone.obstaclePrefabs[Random.Range(0, currentZone.obstaclePrefabs.Count)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaWidth/2, spawnAreaWidth/2), lastSpawnHeight + 10f, 0);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnEnemies()
    {
        if (currentZone.enemyPrefabs.Count > 0)
        {
            GameObject enemyPrefab = currentZone.enemyPrefabs[Random.Range(0, currentZone.enemyPrefabs.Count)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaWidth/2, spawnAreaWidth/2), lastSpawnHeight + 10f, 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnArtifacts()
    {
        if (currentZone.artifactPrefabs.Count > 0 && Random.value < 0.3f) // 30% chance to spawn an artifact
        {
            GameObject artifactPrefab = currentZone.artifactPrefabs[Random.Range(0, currentZone.artifactPrefabs.Count)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaWidth/2, spawnAreaWidth/2), lastSpawnHeight + 10f, 0);
            Instantiate(artifactPrefab, spawnPosition, Quaternion.identity);
        }
    }
}