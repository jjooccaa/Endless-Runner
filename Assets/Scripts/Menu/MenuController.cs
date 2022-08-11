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

    public string newGame;

    private void Start()
    {
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        if(PlayerPrefs.HasKey("highScore")) //temporary solution
        {
           highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("highScore");
        }
    }

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(newGame);
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
