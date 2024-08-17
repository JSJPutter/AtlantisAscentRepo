using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string settingsSceneName = "SettingsScene";
    [SerializeField] private string creditsSceneName = "CreditsScene";
    
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        Debug.Log("MainMenu Start method called");
        // Ensure only the main panel is active at start
        ActivatePanel(mainPanel);
        AddDebugListenersToButtons();
    }

    private void AddDebugListenersToButtons()
    {
        AddDebugListenerToButton("StartGameButton", StartGame);
        AddDebugListenerToButton("SettingsButton", OpenSettings);
        AddDebugListenerToButton("CreditsButton", OpenCredits);
        AddDebugListenerToButton("QuitButton", QuitGame);
    }

    private void AddDebugListenerToButton(string buttonName, UnityEngine.Events.UnityAction action)
    {
        Button button = GameObject.Find(buttonName)?.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => Debug.Log($"{buttonName} clicked"));
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning($"Button {buttonName} not found");
        }
    }

    private void ActivatePanel(GameObject panelToActivate)
    {
        mainPanel.SetActive(panelToActivate == mainPanel);
        settingsPanel.SetActive(panelToActivate == settingsPanel);
        creditsPanel.SetActive(panelToActivate == creditsPanel);
    }

    public void StartGame()
    {
        Debug.Log("StartGame method called");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings method called");
        ActivatePanel(settingsPanel);
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings method called");
        ActivatePanel(mainPanel);
    }

    public void OpenCredits()
    {
        Debug.Log("OpenCredits method called");
        ActivatePanel(creditsPanel);
    }

    public void CloseCredits()
    {
        Debug.Log("CloseCredits method called");
        ActivatePanel(mainPanel);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame method called");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}