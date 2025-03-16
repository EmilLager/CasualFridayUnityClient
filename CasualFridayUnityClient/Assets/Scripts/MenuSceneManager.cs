using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    private const string HighscoreKey = "Highscore";
    
    //Would usually use some kind of DI instead of a direct reference and probably maintain a none-scene specific version of this component using a DontDestroyOnLoad
    [SerializeField] private ScreenFade screenFade;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private string highscoreTextFormat;
    
    //Would usually use some kind of DI instead of a direct reference
    [SerializeField] private AssetReferenceDatabase assetReferenceDatabase;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayButtonPressed);
    }

    private void OnEnable()
    {
        highscoreText.text = string.Format(highscoreTextFormat, PlayerPrefs.GetInt(HighscoreKey, 0));
    }
    
    private async void PlayButtonPressed()
    {
        await assetReferenceDatabase.LoadGameScene(false);
        await screenFade.FadeImageAsync(0f, 1f);
        await assetReferenceDatabase.ActivateGameScene();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayButtonPressed);
    }
}
