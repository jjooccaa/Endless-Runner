using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class ProgressiveRewards : MonoBehaviour 
{
	const string CHECKIN_FUNCTION = "CheckIn";

	public void CheckIn()
	{
		Debug.Log("Checking-in with Server...");
		ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest() 
		{ 
			FunctionName = CHECKIN_FUNCTION, 
		};
		
		PlayFabClientAPI.ExecuteCloudScript(request, OnCheckInCallback, OnApiCallError);
	}
	
	void OnCheckInCallback(ExecuteCloudScriptResult result)
	{
		// output any errors that happend within cloud script
		if(result.Error != null)
		{
			Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
			return;
		}	

		Debug.Log("CheckIn Results:");
;
		var serializer = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
		List<ItemInstance> grantedItems = serializer.DeserializeObject<List<ItemInstance>>(result.FunctionResult.ToString());

		if (grantedItems != null && grantedItems.Count > 0)
		{
			Debug.Log(string.Format("You were granted {0} items:", grantedItems.Count));
			
			string output = string.Empty;
			foreach(var item in grantedItems)
			{
				output += string.Format("\t {0}: {1}\n", item.ItemId, item.Annotation);
			}
			Debug.Log(output);
		}
		else if(result.Logs.Count > 0)
		{
			foreach(var statement in result.Logs)
			{
				Debug.Log(statement.Message);
			}
		}
		else
		{
			Debug.Log("CheckIn Successful! No items granted.");
		}
	}
	
	void OnApiCallError(PlayFabError err)
	{
		string http = string.Format("HTTP:{0}", err.HttpCode);
		string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
		string details = string.Empty;
		
		if(err.ErrorDetails != null)
		{
			foreach(var detail in err.ErrorDetails)
			{
				details += string.Format("{0} \n", detail.ToString());
			}
		}
		
		Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
	}
}
