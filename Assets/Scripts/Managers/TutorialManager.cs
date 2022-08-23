using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject[] tutorialScreens;
    [SerializeField] TextMeshProUGUI continueText;

    [SerializeField] GameObject startTutorialTrigger;

    bool inputDisabled = false;

    int tutorialCounter = 0;
    int delay = 2;

    private void OnEnable()
    {
        EventManager.Instance.onTutorialTrigger += StartNextTutorial;
    }

    private void Update()
    {
        CheckForInputToContinue();
    }

    void StartNextTutorial()
    {
        if (tutorialCounter == tutorialScreens.Length - 1)
        {
            tutorialScreens[tutorialCounter].gameObject.SetActive(true);
            inputDisabled = true;
        }
        else
        {
            inputDisabled = true;
            tutorialScreens[tutorialCounter].gameObject.SetActive(true);
            tutorialCounter++;

            StartCoroutine(ContinueTextAndInputDelay());
        }
    }
    IEnumerator ContinueTextAndInputDelay()
    {
        yield return new WaitForSecondsRealtime(delay);
        continueText.gameObject.SetActive(true);
        inputDisabled = false;
    }

    void CheckForInputToContinue()
    {
        if(!inputDisabled && Input.anyKeyDown)
        {
            Continue();
        }
    }

    void Continue()
    {
        GameManager.Instance.UnpauseMovement();
        tutorialScreens[tutorialCounter - 1].gameObject.SetActive(false);
        continueText.gameObject.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
