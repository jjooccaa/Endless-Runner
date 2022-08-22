using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] TextMeshProUGUI volumeLabel;
    [SerializeField] Slider volumeSlider;

    [SerializeField] TextMeshProUGUI highScoreText;

    public const string MASTER_VOLUME = "masterVolume";

    private void Start()
    {
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        if (PlayerPrefs.HasKey(ScoreManager.HIGH_SCORE))
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetFloat(ScoreManager.HIGH_SCORE);
        }
    }

    // When yes has been pressed in Tutorial dialogue. Load tutorial.
    public void TutorialDialogueYes()
    {
        SceneManager.LoadScene(SceneName.TUTORIAL);
    }

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(SceneName.ENDLESS_RUNNER_GAME);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ApplyVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeLabel.text = volume.ToString();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME, AudioListener.volume);
    }
}
