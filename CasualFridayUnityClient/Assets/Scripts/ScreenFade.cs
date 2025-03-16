using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//Very simplistic scene transition animation
public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeTime;

    private async void Awake()
    {
        await FadeImageAsync(1f, 0f);
    }

    public async Task FadeImageAsync(float startValue, float endValue)
    {
        fadeCanvas.alpha = startValue;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startValue, endValue, elapsedTime / fadeTime);
            
            await Task.Yield();
        }

        fadeCanvas.alpha = endValue;
    }
}
