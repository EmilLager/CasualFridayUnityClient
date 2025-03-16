using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    
    //Used PlayerPrefs and implemented value saving here for convenience but would usually put this somewhere else with a more robust solution
    private const string DifficultyKey = "DifficultySetting";
    private const string SoundKey = "SoundSetting";
    
    [SerializeField] private RectTransform settingsMenu;
    
    [SerializeField] private Button closeButton;
    [SerializeField] private Button closeBackdrop;
    [SerializeField] private Button soundToggleButton; //Could also use a UI toggle for this
    [SerializeField] private Button easyButton; //Could also use a UI toggle for this
    [SerializeField] private Button mediumButton; //Could also use a UI toggle for this
    [SerializeField] private Button hardButton; //Could also use a UI toggle for this
    
    [SerializeField] private Color enabledColor;
    [SerializeField] private Color disabledColor;

    private Difficulty currentDifficulty;
    private bool currentSoundOn;
    
    private void Awake()
    {
        closeButton.onClick.AddListener(CloseSettings);
        closeBackdrop.onClick.AddListener(CloseSettings);
        soundToggleButton.onClick.AddListener(ToggleSound);
        easyButton.onClick.AddListener(SetEasyDifficulty);
        mediumButton.onClick.AddListener(SetMediumDifficulty);
        hardButton.onClick.AddListener(SetHardDifficulty);
        CloseSettings();
    }

    private void OnEnable()
    {
        currentDifficulty = (Difficulty) PlayerPrefs.GetInt(DifficultyKey, 0);
        currentSoundOn = PlayerPrefs.GetInt(SoundKey, 0) == 0;
        UpdateDifficultyButtons(currentDifficulty);
        UpdateSoundButton(currentSoundOn);
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(CloseSettings);
        closeBackdrop.onClick.RemoveListener(CloseSettings);
        soundToggleButton.onClick.RemoveListener(ToggleSound);
        easyButton.onClick.RemoveListener(SetEasyDifficulty);
        mediumButton.onClick.RemoveListener(SetMediumDifficulty);
        hardButton.onClick.RemoveListener(SetHardDifficulty);
    }
    
    public void OpenSettings()
    {
        settingsMenu.gameObject.SetActive(true);
    }
    
    public void CloseSettings()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    private void ToggleSound()
    {
        currentSoundOn = !currentSoundOn;
        PlayerPrefs.SetInt(SoundKey, currentSoundOn? 0 : 1);
        UpdateSoundButton(currentSoundOn);
    }
    
    private void UpdateSoundButton(bool soundOn)
    {
        soundToggleButton.image.color = soundOn ? enabledColor : disabledColor;
    }
    
    private void SetEasyDifficulty()
    {
        currentDifficulty = Difficulty.Easy;
        UpdateDifficultyButtons(currentDifficulty);
        PlayerPrefs.SetInt(DifficultyKey, (int) currentDifficulty);
    }

    private void SetMediumDifficulty()
    {
        currentDifficulty = Difficulty.Medium;
        UpdateDifficultyButtons(currentDifficulty);
        PlayerPrefs.SetInt(DifficultyKey, (int) currentDifficulty);
    }
    
    private void SetHardDifficulty()
    {
        currentDifficulty = Difficulty.Hard;
        UpdateDifficultyButtons(currentDifficulty);
        PlayerPrefs.SetInt(DifficultyKey, (int) currentDifficulty);
    }
    
    private void UpdateDifficultyButtons(Difficulty difficulty)
    {
        easyButton.image.color = difficulty == Difficulty.Easy ? disabledColor : enabledColor;
        mediumButton.image.color = difficulty == Difficulty.Medium ? disabledColor : enabledColor;
        hardButton.image.color = difficulty == Difficulty.Hard ? disabledColor : enabledColor;
    }

    public enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }
}
