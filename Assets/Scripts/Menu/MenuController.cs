using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Graphics Settings")]
    [SerializeField] public Slider brightnessSlider;
    [SerializeField] TMP_Text brightnessValueText;
    [SerializeField] float defaultBrightness = 5;
    [SerializeField] public Toggle fullScreenToggle;
    float brightnessLevel;
    bool isFullScreen;

    [Header("Sound Settings")]
    [SerializeField] public Slider masterVolumeSlider;
    [SerializeField] TMP_Text masterVolumeValue;
    [SerializeField] float defaultVolume = 5;

    [SerializeField] public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        ClearChallengeModes();
    }

    void ClearChallengeModes()
    {
        ChallengeMode.IsEasyModeActive = false;
        ChallengeMode.IsMediumModeActive = false;
        ChallengeMode.IsHardModeActive = false;
    }

    // When yes has been pressed in Tutorial dialogue. Load tutorial.
    public void TutorialDialogueYes()
    {
        LoadScene(SceneName.TUTORIAL);
    }

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        LoadScene(SceneName.ENDLESS_RUNNER_GAME);
    }

    // When yes has been pressed in challenge dialogues. Load new challange.
    public void NewEasyChallengeDialogueYes()
    {
        ChallengeMode.IsEasyModeActive = true;
        LoadScene(SceneName.ENDLESS_RUNNER_GAME);
    }

    public void NewMediumChallengeDialogueYes()
    {
        ChallengeMode.IsMediumModeActive = true;
        LoadScene(SceneName.ENDLESS_RUNNER_GAME);
    }

    public void NewHardChallengeDialogueYes()
    {
        ChallengeMode.IsHardModeActive = true;
        LoadScene(SceneName.ENDLESS_RUNNER_GAME);
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    // Graphics settings
    public void ApplyBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessValueText.text = brightness.ToString();
    }

    public void SetFullScreen(bool fullScreen)
    {
        isFullScreen = fullScreen;
    }

    public void SaveGraphicsSettings()
    {
        PlayerPrefs.SetFloat(LoadPrefs.MASTER_BRIGHTNESS, brightnessLevel);
        Screen.brightness = brightnessLevel;

        PlayerPrefs.SetInt(LoadPrefs.MASTER_FULLSCREEN, (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;
    }

    public void ResetGraphicsButton()
    {
        brightnessSlider.value = defaultBrightness;
        brightnessValueText.text = defaultBrightness.ToString();
        Screen.brightness = defaultBrightness;

        fullScreenToggle.isOn = true;
        Screen.fullScreen = true;
        SaveGraphicsSettings();
    }

    // Sound settings
    public void ApplyMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        masterVolumeValue.text = volume.ToString();
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(LoadPrefs.MASTER_VOLUME, AudioListener.volume);
    }

    public void ResetVolumeButton()
    {
        AudioListener.volume = defaultVolume;
        masterVolumeSlider.value = defaultVolume;
        masterVolumeValue.text = defaultVolume.ToString();
        SaveVolumeSettings();
    }
}
