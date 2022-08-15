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

    private void Start()
    {
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        if(PlayerPrefs.HasKey(ScoreManager.High_Score))
        {
           highScoreText.text = "High Score: " + PlayerPrefs.GetFloat(ScoreManager.High_Score);
        }
    }

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(SceneName.Endless_Runner_Game);
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
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
