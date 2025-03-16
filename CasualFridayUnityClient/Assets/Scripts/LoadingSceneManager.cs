using System.Threading.Tasks;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceDatabase assetDatabase; 
    // Start is called before the first frame update
    private async void Start()
    {
        await Task.WhenAll(
            assetDatabase.LoadAssets(),
            assetDatabase.LoadMenuScene(false));
        
        await assetDatabase.ActivateMenuScene();

        //Trying to load both scenes in advance was troublesome and I decided to leave it aside - described in more detail in AssetReferenceDatabase.cs
        await assetDatabase.LoadGameScene(false);
    }
}
