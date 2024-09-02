using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float depletionRate = 5f; // Oxygen depleted per second
    [SerializeField] private Image oxygenBar;

    [Header("Oxygen Bar Colors")]
    [SerializeField] private Color fullOxygenColor = Color.green;
    [SerializeField] private Color emptyOxygenColor = Color.red;

    private float currentOxygen;

    private void Start()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenBar();
    }

    private void Update()
    {
        // Deplete oxygen over time
        currentOxygen -= depletionRate * Time.deltaTime;
        currentOxygen = Mathf.Max(0f, currentOxygen);

        UpdateOxygenBar();

        if (IsEmpty())
        {
            GameOver();
        }
    }

    public void AddOxygen(float amount)
    {
        currentOxygen = Mathf.Min(maxOxygen, currentOxygen + amount);
        UpdateOxygenBar();
    }

    public void RemoveOxygen(float amount)
    {
        currentOxygen = Mathf.Max(0f, currentOxygen - amount);
        UpdateOxygenBar();
    }

    public bool IsEmpty()
    {
        return currentOxygen <= 0f;
    }

    private void UpdateOxygenBar()
    {
        if (oxygenBar != null)
        {
            float fillAmount = currentOxygen / maxOxygen;
            oxygenBar.fillAmount = fillAmount;

            // Update the color of the oxygen bar
            oxygenBar.color = Color.Lerp(emptyOxygenColor, fullOxygenColor, fillAmount);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over - Out of Oxygen!");
        // Implement game over logic here
    }
}
