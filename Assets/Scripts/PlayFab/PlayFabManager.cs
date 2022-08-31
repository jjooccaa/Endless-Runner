using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    const string TITLE_ID = "26DA0";
    const string HIGHSCORE_LEADERBOARD_NAME = "HighScoreLeaderboard";
    public const string COINS_CODE = "CO";
    public const string GEMS_CODE = "GM";
    const string CHECKIN_FUNCTION = "CheckIn";

    private void OnEnable()
    {
        EventManager.Instance.onRegister += Register;
        EventManager.Instance.onLogin += Login;
        EventManager.Instance.onResetPassword += ResetPassword;
        EventManager.Instance.onSendLeaderboard += SendLeaderboard;
        EventManager.Instance.onGrantCoins += GrantCoins;
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
            EventManager.Instance.onLeaderboardGet?.Invoke(item.Position, item.PlayFabId, item.StatValue.ToString());
        }
    }

    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency[COINS_CODE];
        int gems = result.VirtualCurrency[GEMS_CODE];

        EventManager.Instance.onGetCurrency?.Invoke(coins, gems);
    }

    public void GrantCoins(int amount)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = COINS_CODE,
            Amount = amount
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGrantCoinsSuccess, OnError);
    }

    void OnGrantCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Coins granted!");
    }

    // Progressive Rewards
    public void CheckIn()
    {
        Debug.Log("Checking-in with Server...");
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = CHECKIN_FUNCTION,
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCheckInCallback, OnError);
    }

    void OnCheckInCallback(ExecuteCloudScriptResult result)
    {
        // output any errors that happend within cloud script
        if (result.Error != null)
        {
            Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
            return;
        }

        Debug.Log("CheckIn Results:");

        var serializer = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
        List<ItemInstance> grantedItems = serializer.DeserializeObject<List<ItemInstance>>(result.FunctionResult.ToString());

        if (grantedItems != null && grantedItems.Count > 0)
        {
            Debug.Log(string.Format("You were granted {0} items:", grantedItems.Count));

            string output = string.Empty;
            foreach (var item in grantedItems)
            {
                output += string.Format("\t {0}: {1}\n", item.ItemId, item.Annotation);
            }
            Debug.Log(output);
        }
        else if (result.Logs.Count > 0)
        {
            foreach (var statement in result.Logs)
            {
                Debug.Log(statement.Message);
            }
        }
        else
        {
            Debug.Log("CheckIn Successful! No items granted.");
        }
    }

    public void OnError(PlayFabError error)
    {
        string http = string.Format("HTTP:{0}", error.HttpCode);
        string message = string.Format("ERROR:{0} -- {1}", error.Error, error.ErrorMessage);
        string details = string.Empty;

        if (error.ErrorDetails != null)
        {
            foreach (var detail in error.ErrorDetails)
            {
                details += string.Format("{0} \n", detail.ToString());
            }
        }

        Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
    }
}

