using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private const string SaveFileName = "atlantisascent.save";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class GameData
    {
        public float highestReachedHeight;
        public int totalArtifactsCollected;
        public float bestScore;
        public string[] unlockedUpgrades;
        // Add any other data you want to save
    }

    public void SaveGame(GameData data)
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        FileStream stream = new FileStream(path, FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game saved successfully");
    }

    public GameData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            Debug.Log("Game loaded successfully");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found. Starting a new game.");
            return new GameData();
        }
    }

    public void DeleteSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.LogWarning("No save file found to delete");
        }
    }
}