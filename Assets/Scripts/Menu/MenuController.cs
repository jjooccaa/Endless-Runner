using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] TextMeshProUGUI volumeTextValue;
    [SerializeField] Slider volumeSlider;

    public string newGame;

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(newGame);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void ApplyVolume()
    {
        // Apply and save volume
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
