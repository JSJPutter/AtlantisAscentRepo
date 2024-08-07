using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI zoneText;
   public HealthBar healthBar;

    [Header("Menus")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;

    [Header("Camera Follow")]
    public Camera mainCamera;
    public RectTransform hudPanel;
    public Vector2 hudOffset = new Vector2(0, 10); // Offset from top-left corner

    private void Update()
    {
        if (mainCamera != null && hudPanel != null)
        {
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(mainCamera.transform.position);
            hudPanel.position = screenPosition + hudOffset;
        }
    }

    public void UpdateScoreDisplay(float score)
    {
        scoreText.text = $"Score: {score:F0}";
    }

    public void UpdateHeightDisplay(float height)
    {
        heightText.text = $"Height: {height:F1}m";
    }

    public void UpdateZoneDisplay(string zoneName)
    {
        zoneText.text = $"Zone: {zoneName}";
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
      
        healthBar.SetHealth(currentHealth);
    }
    public void UpdateMaxHealthDisplay(int maxHealth)
    {
        healthBar.SetMaxHealth(maxHealth);
       
    }
    public void ShowMainMenu()
    {
        // mainMenuPanel.SetActive(true);
        // pauseMenuPanel.SetActive(false);
        // gameOverPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        // mainMenuPanel.SetActive(false);
        // pauseMenuPanel.SetActive(true);
        // gameOverPanel.SetActive(false);
    }

    public void HidePauseMenu()
    {
        // pauseMenuPanel.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if(gameOverPanel!=null) gameOverPanel.SetActive(true);
    }
}