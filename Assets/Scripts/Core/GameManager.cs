using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentGameState { get; private set; }

    public float CurrentScore { get; private set; }
    public float HighestReachedHeight { get; private set; }

    public PlayerController Player { get; private set; }

    public CameraController CameraController { get; private set; }
    public WaterManager WaterManager { get; private set; }

    public UIManager UIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public ZoneManager ZoneManager { get; private set; }

    private Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        CameraController = FindObjectOfType<CameraController>();
        WaterManager = FindObjectOfType<WaterManager>();
        Player = FindObjectOfType<PlayerController>();

        UIManager = FindObjectOfType<UIManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        ZoneManager = FindObjectOfType<ZoneManager>();

        if (CameraController == null || WaterManager == null || UIManager == null || AudioManager == null || ZoneManager == null)
        {
            Debug.LogError("One or more required managers are missing in the scene.");
        }
    }

     public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        // SpawnPlayer();
        ResetScore();
        InitializeZoneManager();
    }

    private void InitializeZoneManager()
    {
        if (ZoneManager != null && Player != null)
        {
            // Ensure the ZoneManager is aware of the initial player position
            ZoneManager.UpdatePlayerHeight(Player.transform.position.y);
        }
        else
        {
            Debug.LogError("ZoneManager or Player is null during game start.");
        }
    }

    // private void SpawnPlayer()
    // {
    //     GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    //     if (playerPrefab != null)
    //     {
    //         GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //         Player = playerObject.GetComponent<PlayerController>();
    //         if (Player == null)
    //         {
    //             Debug.LogError("PlayerController component not found on the player prefab.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("Player prefab not found in Resources/Prefabs/Player");
    //     }
    // }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.Playing)
        {
            CurrentGameState = GameState.Paused;
            Time.timeScale = 0;
            UIManager.ShowPauseMenu();
        }
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.Paused)
        {
            CurrentGameState = GameState.Playing;
            Time.timeScale = 1;
            // UIManager.HidePauseMenu();
        }
    }

    public void GameOver()
    {
        CurrentGameState = GameState.GameOver;
        UIManager.ShowGameOverMenu();
        // Save high score, etc.
    }

    public void AddScore(float points)
    {
        CurrentScore += points;
        UIManager.UpdateScoreDisplay(CurrentScore);
    }

    public void UpdateHeight(float height)
    {
        if (height > HighestReachedHeight)
        {
            HighestReachedHeight = height;
            UIManager.UpdateHeightDisplay(HighestReachedHeight);
        }
        ZoneManager.UpdatePlayerHeight(height);
    }

    public void SaveGame()
    {
        // Implement save game logic
    }

    public void LoadGame()
    {
        // Implement load game logic
    }

    // Event System
    public void AddListener(string eventName, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener;
        }
    }

    public void RemoveListener(string eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, object data = null)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].Invoke(data);
        }
    }

    private void ResetScore()
    {
        CurrentScore = 0;
        HighestReachedHeight = 0;
        UIManager.UpdateScoreDisplay(CurrentScore);
        UIManager.UpdateHeightDisplay(HighestReachedHeight);
    }

    // Handle zone changes
    private void OnZoneChanged(object data)
    {
        if (data is ZoneData newZone)
        {
            // Handle zone transition effects, update UI, etc.
            // UIManager.UpdateZoneDisplay(newZone.zoneName);
            // AudioManager.PlayZoneTransitionSound();
            // Add any other zone transition logic here
        }
    }

    private void OnEnable()
    {
        AddListener("ZoneChanged", OnZoneChanged);
    }

    private void OnDisable()
    {
        RemoveListener("ZoneChanged", OnZoneChanged);
    }
}