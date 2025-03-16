using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

//This SO is used as a database for loaded assets allowing us to load them once on startup and then use them directly afterwards
//I would usually use some kind of DI system to access this instead of a direct reference or singleton. Like a ServiceLocator pattern for example.
[CreateAssetMenu(fileName = "AssetReferenceDatabase", menuName = "Settings/AssetReferenceDatabase")]
public class AssetReferenceDatabase : ScriptableObject
{
    [SerializeField] private AssetReference  menuSceneReference;
    [SerializeField] private AssetReference  gameSceneReference;
    [SerializeField] private List<AssetReferenceBallController> ballReferences;

    private Dictionary<int, BallController> ballPrefabDictionary;
    
    private AsyncOperationHandle<SceneInstance> menuSceneLoadHandle;
    private AsyncOperationHandle<SceneInstance> gameSceneLoadHandle;
    
    private SceneInstance menuSceneInstance;
    private SceneInstance gameSceneInstance;

    public async Task LoadAssets()
    {
        await LoadAllBallAssets();
    }
    
    //Really wanted to preload both scenes during initial loading without activating them but ran into a known issue with scenes specifically
    //In a real project I would try to load the game scene before transitioning to the menu scene
    public async Task LoadMenuScene(bool activate = true)
    {
        if (!menuSceneLoadHandle.IsValid())
        {
            menuSceneLoadHandle = await LoadSceneAsync(menuSceneReference);
            menuSceneInstance = menuSceneLoadHandle.Result;
        }

        if (activate)
        {
            menuSceneInstance.ActivateAsync();    
        }
    }
    
    public async Task LoadGameScene(bool activate = true)
    {
        if (!gameSceneLoadHandle.IsValid())
        {
            gameSceneLoadHandle = await LoadSceneAsync(gameSceneReference);
            gameSceneInstance = gameSceneLoadHandle.Result;
        }
        if (activate)
        {
            gameSceneInstance.ActivateAsync();    
        }
    }

    //Activating scenes should not be managed by this script and should be delegated to a proper manager in a real case.
    public async Task ActivateMenuScene()
    {
        await ActivateSceneAsync(menuSceneInstance);
    }

    public async Task ActivateGameScene()
    {
        await ActivateSceneAsync(gameSceneInstance);
    }

    private async Task ActivateSceneAsync(SceneInstance sceneInstance)
    {
        var tcs = new TaskCompletionSource<bool>();
        AsyncOperation sceneLoadHandle = sceneInstance.ActivateAsync();

        sceneLoadHandle.completed += (handle) =>
        {
            tcs.SetResult(true);
        };

        await tcs.Task;
    }

    private async Task LoadAllBallAssets()
    {
        await Task.WhenAll(ballReferences.Select(LoadAssetAsync));
    }
    
    private async Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(AssetReference sceneReference)
    {
        var tcs = new TaskCompletionSource<SceneInstance>();
        AsyncOperationHandle<SceneInstance> sceneLoadHandle = sceneReference.LoadSceneAsync(LoadSceneMode.Single, false);

        sceneLoadHandle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                tcs.SetResult(handle.Result);
            }
            else
            {
                Debug.LogError("Failed to load scene: " + handle.Status);
                tcs.SetResult(new SceneInstance());
            }
        };

        await tcs.Task;
        return sceneLoadHandle;
    }
    
    private async Task<T> LoadAssetAsync<T>(AssetReferenceT<T> reference) where T : Object
    {
        var tcs = new TaskCompletionSource<T>();
        AsyncOperationHandle<T> loadControllerHandle = reference.LoadAssetAsync<T>();

        loadControllerHandle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                //ballPrefabDictionary.Add(ballPrefabDictionary.Count, handle.Result);
                tcs.SetResult(handle.Result);
            }
            else
            {
                Debug.LogError("Failed to load ball prefab: " + handle.Status);
                tcs.SetResult(null);
            }
        };
        return await tcs.Task;
    }
    
    
    [Serializable]
    public class AssetReferenceBallController : AssetReferenceT<BallController>
    {
        public AssetReferenceBallController(string guid) : base(guid)
        {
        }
    }
}
