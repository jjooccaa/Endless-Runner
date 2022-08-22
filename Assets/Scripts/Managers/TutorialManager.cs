using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialScreens;
    [SerializeField] GameObject startTutorialTrigger;

    int tutorialCounter = 0;

    private void OnEnable()
    {
        EventManager.Instance.onTutorialTrigger += StartNextTutorial;
    }

    void StartNextTutorial()
    {
        tutorialScreens[tutorialCounter].gameObject.SetActive(true);
        tutorialCounter++;
    }

    public void Continue()
    {
        GameManager.Instance.UnpauseMovement();
        tutorialScreens[tutorialCounter - 1].gameObject.SetActive(false);
    }

    public void Ext()
    {
        SceneManager.LoadScene(SceneName.MAIN_MENU);
    }
}
