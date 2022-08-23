using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [SerializeField] bool canUse = false;
    [SerializeField] MenuController menuController;

    public const string MASTER_VOLUME = "masterVolume";
    public const string MASTER_BRIGHTNESS = "masterBrightness";
    public const string MASTER_FULLSCREEN = "masterFullscreen";

    private void Awake()
    {
        if (canUse && menuController != null)
        {
            LoadHighScore();
            LoadMasterVolume();
            LoadMasterBrightness();
            LoadMasterFullScreen();
        }
    }

    void LoadHighScore()
    {
        if (PlayerPrefs.HasKey(ScoreManager.HIGH_SCORE))
        {
            menuController.highScoreText.text = "High Score: " + PlayerPrefs.GetFloat(ScoreManager.HIGH_SCORE);
        }
    }

    void LoadMasterVolume()
    {
        if (PlayerPrefs.HasKey(MASTER_VOLUME))
        {
            float localVolume = PlayerPrefs.GetFloat(MASTER_VOLUME);

            menuController.ApplyMasterVolume(localVolume);
            menuController.masterVolumeSlider.value = localVolume;
        }
        else
        {
            menuController.ResetVolumeButton();
        }
    }

    void LoadMasterBrightness()
    {
        if (PlayerPrefs.HasKey(MASTER_BRIGHTNESS))
        {
            float localBrightness = PlayerPrefs.GetFloat(MASTER_BRIGHTNESS);

            menuController.ApplyBrightness(localBrightness);
            menuController.brightnessSlider.value = localBrightness;
        }
    }

    void LoadMasterFullScreen()
    {
        if (PlayerPrefs.HasKey(MASTER_FULLSCREEN))
        {
            int localFullscreen = PlayerPrefs.GetInt(MASTER_FULLSCREEN);

            if (localFullscreen == 1)
            {
                Screen.fullScreen = true;
                menuController.fullScreenToggle.isOn = true;
            }
            else
            {
                Screen.fullScreen = false;
                menuController.fullScreenToggle.isOn = false;
            }
        }
    }
}
