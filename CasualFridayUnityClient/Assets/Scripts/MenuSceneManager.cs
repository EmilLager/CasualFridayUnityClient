using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    private const string HighscoreKey = "Highscore";
    
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

    //Method here for easy addition of button feedback if non-immediate transition is required  
    private async void PlayButtonPressed()
    {
        await assetReferenceDatabase.LoadGameScene();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayButtonPressed);
    }
}
