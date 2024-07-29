using System;
using System.Collections.Generic;
using System.Linq;
using Artifacts;
using ScriptableObjects.ArtifactData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core {
    public class GameManager : MonoBehaviour {
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
        public OxygenSystem OxygenManager { get; private set; }
        
        public ArtifactManager ArtifactManager { get; private set; }

        private Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

        private TutorialManager tutorialManager;
        private bool isTutorialComplete = false;
        
        private void Awake() {
            Time.timeScale = 1f;
            if (Instance == null) {
                Instance = this;
              //  DontDestroyOnLoad(gameObject);
                InitializeManagers();
                CurrentGameState = GameState.Playing;
            } else {
                Destroy(gameObject);
            }
            ResetScore();
        }

        private void InitializeManagers() {
            var managers = new MonoBehaviour[] {
                CameraController = FindObjectOfType<CameraController>(),
                WaterManager = FindObjectOfType<WaterManager>(),
                Player = FindObjectOfType<PlayerController>(),
                UIManager = FindObjectOfType<UIManager>(),
                AudioManager = FindObjectOfType<AudioManager>(),
                ZoneManager = FindObjectOfType<ZoneManager>(),
                ArtifactManager = FindObjectOfType<ArtifactManager>(),
                tutorialManager = FindObjectOfType<TutorialManager>(),
                OxygenManager = FindObjectOfType<OxygenSystem>()
            };

            // tutorialManager = TutorialManager.Instance;
            // if (tutorialManager == null)
            // {
            //     Debug.LogError("TutorialManager not found in the scene!");
            // }
            // else
            // {
            //     StartTutorial();
            // }
            //
            if (managers.Any(manager => manager == null)) {
                Debug.LogError("One or more required managers are missing in the scene.");
            }
           
        }

        public void StartTutorial()
        {
            if (!isTutorialComplete && tutorialManager != null)
            {
                tutorialManager.StartTutorial();
            }
        }
        
        public void TutorialCompleted()
        {
            isTutorialComplete = true;
        }
        
        public bool IsGamePaused()
        {
            return CurrentGameState == GameState.Paused;
        }
        
        public void StartGame()
        {
            if (tutorialManager.IsTutorialActive())
            {
                // Wait for tutorial to complete
                return;
            }

            CurrentGameState = GameState.Playing;
            ResetScore();
            InitializeZoneManager();
            
            ArtifactManager.Instance.allArtifacts = Resources.LoadAll<Artifact>("Artifacts").ToList();
        }

        private void InitializeZoneManager() {
            if (ZoneManager != null && Player != null) {
                // Ensure the ZoneManager is aware of the initial player position
                ZoneManager.UpdatePlayerHeight(Player.transform.position.y);
            } else {
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

        public void PauseGame() {
            if (CurrentGameState == GameState.Playing) {
                CurrentGameState = GameState.Paused;
                Time.timeScale = 0;
                UIManager.ShowPauseMenu();
            }
        }

        public void ResumeGame() {
            if (CurrentGameState == GameState.Paused) {
                CurrentGameState = GameState.Playing;
                Time.timeScale = 1;
                UIManager.HidePauseMenu();
            }
        }

        public void GameOver() {
            CurrentGameState = GameState.GameOver;
            UIManager.ShowGameOverMenu();
            Time.timeScale = 0;
            // Save high score, etc.
        }
        public void Home()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void AddScore(float points) {
            CurrentScore += points;
            UIManager.UpdateScoreDisplay(CurrentScore);
        }

        public void UpdateHeight(float height) {
            if (height > HighestReachedHeight) {
                HighestReachedHeight = height;
                UIManager.UpdateHeightDisplay(HighestReachedHeight);
            }
            
            ZoneManager.UpdatePlayerHeight(height);
        }

        public void SaveGame() {
            // Implement save game logic
        }

        public void LoadGame() {
            // Implement load game logic
        }

        // Event System
        public void AddListener(string eventName, Action<object> listener) {
            if (!eventDictionary.ContainsKey(eventName)) {
                eventDictionary[eventName] = listener;
            } else {
                eventDictionary[eventName] += listener;
            }
        }

        public void RemoveListener(string eventName, Action<object> listener) {
            if (eventDictionary.ContainsKey(eventName)) {
                eventDictionary[eventName] -= listener;
            }
        }

        public void TriggerEvent(string eventName, object data = null) {
            if (eventDictionary.ContainsKey(eventName)) {
                eventDictionary[eventName].Invoke(data);
            }
        }

        private void ResetScore() {
            CurrentScore = 0;
            HighestReachedHeight = 0;
            UIManager.UpdateScoreDisplay(CurrentScore);
            UIManager.UpdateHeightDisplay(HighestReachedHeight);
        }

        private void OnZoneChanged(object data) {
            if (data is ZoneData newZone) {
                // UIManager.UpdateZoneDisplay(newZone.zoneName);
                // AudioManager.PlayZoneTransitionSound();
            }
        }

        private void OnEnable() {
            AddListener("ZoneChanged", OnZoneChanged);
        }

        private void OnDisable() {
            RemoveListener("ZoneChanged", OnZoneChanged);
        }
        
        
    }
}