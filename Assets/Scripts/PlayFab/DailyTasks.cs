using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class DailyTasks : Singleton<DailyTasks>
{
    const string GET_DAILY_TASK_FUNCTION = "GetDailyTask";
    const string CHECK_TASK_FUNCTION = "CheckTask";

    static string taskInfo;

    private void OnEnable()
    {
        EventManager.Instance.onLoginSuccess = GetDailyTask;
    }

    public void GetDailyTask()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = GET_DAILY_TASK_FUNCTION
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetDailyTaskCallback, OnApiCallError);
    }

    void OnGetDailyTaskCallback(ExecuteCloudScriptResult result)
    {
        if (result.Logs.Count > 0)
        {
            foreach (var statement in result.Logs)
            {
                Debug.Log(statement.Message);
            }
        }

        // output any errors that happend within cloud script
        if (result.Error != null)
        {
            Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
            return;
        }

        if (result != null)
        {
            taskInfo = result.FunctionResult.ToString();
        }
        CheckTaskInfo();
    }

    public void CheckTaskProgress(int enemiesKilled)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = CHECK_TASK_FUNCTION,
            FunctionParameter = new
            {
                enemies = enemiesKilled
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCheckTaskCallback, OnApiCallError);
    }

    void OnCheckTaskCallback(ExecuteCloudScriptResult result)
    {
        if (result.Logs.Count > 0)
        {
            foreach (var statement in result.Logs)
            {
                Debug.Log(statement.Message);
            }
        }
        // output any errors that happend within cloud script
        if (result.Error != null)
        {
            Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
            return;
        }

        if (result != null)
        {
            taskInfo = result.FunctionResult.ToString();
        }
    }

    public void CheckTaskInfo()
    {
        EventManager.Instance.onGetDailyTaskChange?.Invoke(taskInfo);
        Debug.Log(taskInfo + " taskInfo called");
    }

    void OnApiCallError(PlayFabError error)
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
