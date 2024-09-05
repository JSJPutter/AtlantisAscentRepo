using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
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

    private void Start()
    {
        tutorialPanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextStep);
        StartTutorial();
    }

    public void StartTutorial()
    {
        if (tutorialSteps.Count > 0)
        {
            isTutorialActive = true;
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
        tutorialImage.sprite = step.image;
        nextButton.gameObject.SetActive(step.waitForInput);

        if (!step.waitForInput)
        {
            StartCoroutine(AutoAdvanceTutorial(step.displayTime));
        }
    }

    private IEnumerator AutoAdvanceTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextStep();
    }

    private void EndTutorial()
    {
        isTutorialActive = false;
        tutorialPanel.SetActive(false);
    }

    public void TriggerContextualTutorial(string triggerName)
    {
        // if (triggerName == "FirstEnemy" && !hasShownEnemyTutorial) { ... }
    }
}