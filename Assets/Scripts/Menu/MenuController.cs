using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Login screen")]
    [SerializeField] GameObject loginScreen;
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_Text infoMessage;
    [SerializeField] GameObject displayNamePanel;
    [SerializeField] TMP_InputField displayNameInput;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;

    [Header("Daily Rewards")]
    [SerializeField] GameObject dailyRewardsPanel;

    [Header("Daily Task")]
    [SerializeField] GameObject dailyTaskPanel;
    [SerializeField] TMP_Text dailyTaskInfo;

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

    [Header("Leaderboard")]
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowsParent;

    [Header("Currencies")]
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text gemsText;

    [Header("High Score")]
    [SerializeField] public TMP_Text highScoreText;

    static bool isLogged = false;

    private void OnEnable()
    {
        EventManager.Instance.onLoginInfoChange += DisplayLoginInfoText;
        EventManager.Instance.onRegisterSuccess += ActivateDisplayNamePanel;
        EventManager.Instance.onUpdateDisplayNameSuccess += DeactivateDisplayNamePane;
        EventManager.Instance.onLoginSuccess += Loggedin;
        EventManager.Instance.onLoginSuccess += DeactivateLoginScreen;
        EventManager.Instance.onLeaderboardGet += GenerateLeaderboardRow;
        EventManager.Instance.onGetCurrency += DisplayCurrencies;
        EventManager.Instance.onGetDailyTaskChange += DisplayDailyTaskInfo;
    }

    private void Awake()
    {
        ClearChallengeModes();
        if(isLogged)
        {
            DeactivateLoginScreen();
            ActivateDailyTaskPanel();
            ActivateMainMenu();
        }
    }

    void ClearChallengeModes()
    {
        ChallengeMode.IsEasyModeActive = false;
        ChallengeMode.IsMediumModeActive = false;
        ChallengeMode.IsHardModeActive = false;
    }

    void Loggedin()
    {
        isLogged = true;
        ActivateDailyRewardsPanel();
        ActivateDailyTaskPanel();
        ActivateMainMenu();
    }

    void ActivateDailyRewardsPanel()
    {
        dailyRewardsPanel.SetActive(true);
    }

    void ActivateDailyTaskPanel()
    {
        dailyTaskPanel.SetActive(true);
    }

    void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
    }

    void DeactivateLoginScreen()
    {
        loginScreen.SetActive(false);
    }

    #region User Login logic
    public void RegisterButton()
    {
        EventManager.Instance.onRegister?.Invoke(emailInput.text, passwordInput.text);
    }

    void ActivateDisplayNamePanel()
    {
        displayNamePanel.gameObject.SetActive(true);
    }

    void DeactivateDisplayNamePane()
    {
        displayNamePanel.gameObject.SetActive(false);
    }

    public void LoginButton()
    {
        EventManager.Instance.onLogin?.Invoke(emailInput.text, passwordInput.text);
    }

    public void ResetPasswordButton()
    {
        EventManager.Instance.onResetPassword?.Invoke(emailInput.text);
    }

    public void SaveDisplayNameButton()
    {
        EventManager.Instance.onSaveDisplayName?.Invoke(displayNameInput.text);
    }

    void DisplayLoginInfoText(string message)
    {
        infoMessage.text = message;
    }
    #endregion

    void DisplayDailyTaskInfo(string taskInfo)
    {
        dailyTaskInfo.text = taskInfo;
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

    #region Graphic Settings
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
    #endregion

    #region Sound Settings
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
    #endregion

    #region Leaderboard
    void GenerateLeaderboardRow(int position, string name, string score)
    {
        GameObject row = Instantiate(rowPrefab, rowsParent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
        texts[0].text = (position + 1).ToString();
        texts[1].text = name;
        texts[2].text = score;
    }

    public void ClearLaderboard()
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
    }
    #endregion

    void DisplayCurrencies(int coins, int gems)
    {
        coinsText.text = coins.ToString();
        gemsText.text = gems.ToString();
    }
}
