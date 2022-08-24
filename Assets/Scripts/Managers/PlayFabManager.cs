using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    const string TITLE_ID = "26DA0";
    const string HIGHSCORE_LEADERBOARD_NAME = "HighScoreLeaderboard";

    private void OnEnable()
    {
        EventManager.Instance.onRegister += Register;
        EventManager.Instance.onLogin += Login;
        EventManager.Instance.onResetPassword += ResetPassword;
    }

    void Register(string email, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnLoginOrRegisterError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        EventManager.Instance.onLoginInfoChange?.Invoke("Successful registration");
    }

    void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginOrRegisterError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        EventManager.Instance.onLoginInfoChange?.Invoke("Logged in");
        EventManager.Instance.onLoginSuccess?.Invoke();
    }

    void ResetPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = TITLE_ID
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnLoginOrRegisterError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        EventManager.Instance.onLoginInfoChange?.Invoke("Pasword successfully reseted. Check your email");
    }

    void OnLoginOrRegisterError(PlayFabError error)
    {
        EventManager.Instance.onLoginInfoChange?.Invoke(error.ErrorMessage);
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = HIGHSCORE_LEADERBOARD_NAME,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = HIGHSCORE_LEADERBOARD_NAME,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            EventManager.Instance.onLeaderboardGet?.Invoke(item.Position.ToString(), item.PlayFabId, item.StatValue.ToString());
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}

