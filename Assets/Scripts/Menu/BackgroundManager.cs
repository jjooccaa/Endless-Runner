using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] List<AssetReferenceSprite> backgrounds;
    [SerializeField] GameObject currentBackground;

    private AsyncOperationHandle<Sprite> currentBackgroundOperationHandle;
    int backgroundIndex = 0;

    public const string BACKGROUND = "backgroundIndex";

    public void ChangeBackground()
    {
        if (backgroundIndex >= backgrounds.Count - 1)
        {
            backgroundIndex = 0;
        }
        else
        {
            backgroundIndex++;
        }

        SaveBackground();
        StartCoroutine(SetBackgroundInternal(backgroundIndex));
    }

    public void SetBackground(int index)
    {
        if (index <= backgrounds.Count - 1)
        {
            StartCoroutine(SetBackgroundInternal(index));
        }
    }

    void SaveBackground()
    {
        PlayerPrefs.SetInt(BACKGROUND, backgroundIndex);
    }

    IEnumerator SetBackgroundInternal(int backgroundIndex)
    {
        if (currentBackgroundOperationHandle.IsValid())
        {
            Addressables.Release(currentBackgroundOperationHandle);
        }

        var backgroundReference = backgrounds[backgroundIndex];
        currentBackgroundOperationHandle = backgroundReference.LoadAssetAsync<Sprite>();
        yield return currentBackgroundOperationHandle;
        Debug.Log("Background set: " + currentBackgroundOperationHandle.Result);
        currentBackground.GetComponent<Image>().sprite = currentBackgroundOperationHandle.Result;
    }
}
