using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Core;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [System.Serializable]
    public class TutorialStep
    {
        public string message;
        public Sprite image;
        public float displayTime = 5f;
        public bool waitForInput = false;
    }

    public List<TutorialStep> tutorialSteps;
    public GameObject tutorialPanel;
    public Text messageText;
    public Image tutorialImage;
    public Button nextButton;

    private int currentStepIndex = -1;
    private bool isTutorialActive = false;

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
    }

    private void Start()
    {
        tutorialPanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextStep);
        InitializeTutorialSteps();
        StartTutorial();
    }

    private void InitializeTutorialSteps()
    {
        tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep
            {
                message = "Welcome to Atlantis Ascent: Escape the Depths!",
                displayTime = 3f,
                waitForInput = false
            },
            new TutorialStep
            {
                message = "Use WASD or Arrow Keys to move your character.",
                displayTime = 4f,
                waitForInput = false
            },
            new TutorialStep
            {
                message = "Press SPACE to use your blast ability.",
                displayTime = 4f,
                waitForInput = false
            },
            new TutorialStep
            {
                message = "Collect oxygen bubbles to stay alive longer.",
                displayTime = 4f,
                waitForInput = false
            },
            new TutorialStep
            {
                message = "Avoid obstacles and enemies as you ascend.",
                displayTime = 4f,
                waitForInput = false
            },
            new TutorialStep
            {
                message = "Good luck on your journey to the surface!",
                displayTime = 3f,
                waitForInput = true
            }
        };
    }

    public void StartTutorial()
    {
        if (tutorialSteps.Count > 0)
        {
            isTutorialActive = true;
            GameManager.Instance.PauseGame();
            ShowNextStep();
        }
    }

    private void ShowNextStep()
    {
        currentStepIndex++;
        if (currentStepIndex < tutorialSteps.Count)
        {
            DisplayTutorialStep(tutorialSteps[currentStepIndex]);
        }
        else
        {
            EndTutorial();
        }
    }

    private void DisplayTutorialStep(TutorialStep step)
    {
        tutorialPanel.SetActive(true);
        messageText.text = step.message;
        if (step.image != null)
        {
            tutorialImage.sprite = step.image;
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(step.waitForInput);

        if (!step.waitForInput)
        {
            StartCoroutine(AutoAdvanceTutorial(step.displayTime));
        }
    }

    private IEnumerator AutoAdvanceTutorial(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowNextStep();
    }

    private void EndTutorial()
    {
        isTutorialActive = false;
        tutorialPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
        GameManager.Instance.TutorialCompleted();
    }

    public bool IsTutorialActive()
    {
        return isTutorialActive;
    }
}