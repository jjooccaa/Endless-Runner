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
        EventManager.Instance.onLoginSuccess += GetDailyTask;
    }

    public void GetDailyTask()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = GET_DAILY_TASK_FUNCTION
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetDailyTaskCallback, PlayFabManager.Instance.OnError);
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
            RefreshTaskInfo();
        }
    }

    public void RefreshTaskInfo()
    {
        EventManager.Instance.onGetDailyTaskChange?.Invoke(taskInfo);
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

        PlayFabClientAPI.ExecuteCloudScript(request, OnCheckTaskCallback, PlayFabManager.Instance.OnError);
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
}
