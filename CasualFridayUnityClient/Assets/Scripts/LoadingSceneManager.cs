using System.Threading.Tasks;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    
    // Start is called before the first frame update
    private async void Start()
    {
        await Task.Delay(3000);
        _sceneLoader.LoadScene();
    }
}
