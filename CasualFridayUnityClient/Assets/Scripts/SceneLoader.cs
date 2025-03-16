using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[Serializable]
public class SceneLoader
{
    //I would usually use a more robust system for managing scene references 
    [SerializeField] private AssetReference _sceneReference;
    
    //Async Task so that it can be awaited if needed
    public async Task LoadScene(AssetReference sceneReference = null)
    {
        await LoadSceneAsync(sceneReference ?? _sceneReference);
    }

    private async Task LoadSceneAsync(AssetReference sceneReference)
    {
        var tcs = new TaskCompletionSource<SceneInstance>();
        
        AsyncOperationHandle<SceneInstance> loadSceneHandle = sceneReference.LoadSceneAsync();

        loadSceneHandle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                tcs.SetResult(handle.Result);
            }
            else
            {
                tcs.SetException(new Exception("Failed to load addressable scene: " + handle.Status));
            }
        };

        try
        {
            SceneInstance loadedScene = await tcs.Task;
            Debug.Log("Addressable scene loaded using TCS: " + loadedScene.Scene.name);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
