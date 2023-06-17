using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioSource _music;
    [SerializeField]
    private AudioSource _sfx;
    [SerializeField]
    private AudioSource _startButtonSFX;
    [SerializeField]
    private AudioSource _startButtonSFXAlternate;
    [SerializeField]
    private AudioSource _sfxAlternate;
    [SerializeField]
    private GameObject _options;
    [SerializeField]
    Slider musicSliderVal;
    [SerializeField]
    Slider SFXSliderVal;
    [SerializeField]
    private Text _clockText, _altSFXText, _altControlsText;
    [SerializeField]
    private Toggle _clockToggle;
    [SerializeField]
    private Toggle _alternateSFXToggle;
    [SerializeField]
    private Toggle _alternateControlsToggle;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private Toggle _healthBarsToggle;
    [SerializeField]
    private Toggle _subtitlesToggle;
    [SerializeField]
    Button _bossModeButton;
    private bool _pressedStart = false;
    public string UserIdentifier { get; set; }

    private void Awake()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
    }

    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Authenticated. Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
            UserIdentifier = Social.localUser.userName;
            if (UserIdentifier == null || UserIdentifier == "")
                UserIdentifier = "temp";
        }
        else
        {
            UserIdentifier = "temp";
            Debug.Log("*** Failed to authenticate with " + signInStatus);
        }

        // Set up volume
        Debug.Log("In MainMenu::Start, UserIdentifier: " + UserIdentifier);
        _music.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "Music", 0.6f);
        _sfx.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
        _sfxAlternate.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
        _startButtonSFX.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
        _startButtonSFXAlternate.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
        _startButtonSFX.volume += 0.5f;
        float tempStartButtonVol = _startButtonSFX.volume;
        if (tempStartButtonVol + 0.5f > 1f)
            _startButtonSFX.volume = 1f;
        else if (tempStartButtonVol == 0)
            _startButtonSFX.volume = 0;
        musicSliderVal.value = PlayerPrefs.GetFloat(UserIdentifier + "_" + "Music", 0.6f);
        SFXSliderVal.value = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);

        // Enable Boss Mode if the player already beat the game, disable it otherwise
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0) >= 1)
            _bossModeButton.gameObject.SetActive(true);
        else
            _bossModeButton.gameObject.SetActive(false);

        // Set toggles
        int clockDisplay = PlayerPrefs.GetInt(UserIdentifier + "_" + "Clock", 0);
        if (clockDisplay == 1)
            _clockToggle.isOn = true;
        else
            _clockToggle.isOn = false;

        int alternateSFX = PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0);
        if (alternateSFX == 1)
            _alternateSFXToggle.isOn = true;
        else
            _alternateSFXToggle.isOn = false;

        int alternateControls = PlayerPrefs.GetInt(UserIdentifier + "_" + "Alternate_Controls", 0);
        if (alternateControls == 1)
            _alternateControlsToggle.isOn = true;
        else
            _alternateControlsToggle.isOn = false;

        int healthBars = PlayerPrefs.GetInt(UserIdentifier + "_" + "Health Bars", 0);
        if (healthBars == 1)
            _healthBarsToggle.isOn = true;
        else
            _healthBarsToggle.isOn = false;

        int subtitles = PlayerPrefs.GetInt(UserIdentifier + "_" + "Subtitles", 1);
        if (subtitles == 1)
            _subtitlesToggle.isOn = true;
        else
            _subtitlesToggle.isOn = false;
    }

    public void StartButton()
    {
        if (_pressedStart)
            return;

        _pressedStart = true;
        _startButtonSFX.Play();
        PlayerPrefs.SetInt("Boss Mode", 0);

        try
        {
            // Check if the player has beaten the game three times
            int beatTheGameCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0);
            if (beatTheGameCount == 2)
            {
                // Try to unlock the "Addicted" achievement
                DoAchievementUnlock(SmokeTest.GPGSIds.achievement_addicted, (bool achievementUnlocked) =>
                {
                    
                    if (achievementUnlocked)
                    {
                        // Increment the "Completionist" achievement if "Addicted" was unlocked
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_still_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                    
                });
            }
        }
        
        catch (Exception)
        {
            Debug.LogError("There was an error regarding checking for number of times the player beat the game");
        }
        finally
        {
            _anim.SetTrigger("Start");
            StartCoroutine(FadeMusicOut());
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
        if (_pressedStart)
            return;

        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        Application.Quit();
    }

    public void MenuButton()
    {
        if (_pressedStart)
            return;

        _options.SetActive(true);
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();
    }

    // Options related methods
    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void ShowLeaderBoard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(SmokeTest.GPGSIds.leaderboard_hall_of_fame);
    }

    public void BackButton()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        _options.SetActive(false);
    }

    public void MusicSlider()
    {
        float val = musicSliderVal.value;
        _music.volume = val;
        PlayerPrefs.SetFloat(UserIdentifier + "_" + "Music", val);
    }

    public void SFXSlider()
    {
        float val = SFXSliderVal.value;
        _sfx.volume = val;
        _sfxAlternate.volume = val;
        _startButtonSFX.volume = val;
        _startButtonSFXAlternate.volume = val;
        PlayerPrefs.SetFloat(UserIdentifier + "_" + "SFX", val);
    }

    public void RestoreToDefaultsButton()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        // Set the defaults
        musicSliderVal.value = 0.6f;
        SFXSliderVal.value = 0.6f;
        _music.volume = 0.6f;
        _sfx.volume = 0.6f;
        _sfxAlternate.volume = 0.6f;
        _startButtonSFX.volume = 0.6f;
        _startButtonSFXAlternate.volume = 0.6f;
        _healthBarsToggle.isOn = false;
        _subtitlesToggle.isOn = true;
        _clockToggle.isOn = false;
        _alternateSFXToggle.isOn = false;
        _alternateControlsToggle.isOn = false;

        int beatTheGameCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0);
        int deathCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "DeathCount", 0);
        PlayerPrefs.DeleteAll();

        // Restore BeatTheGameCount and DeathCount
        PlayerPrefs.SetInt(UserIdentifier + "_" + "BeatTheGameCount", beatTheGameCount);
        PlayerPrefs.SetInt(UserIdentifier + "_" + "DeathCount", deathCount);
    }

    public void ClockToggle()
    {
        if (_clockToggle.isOn)
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Clock", 1);
        }
        else
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Clock", 0);
        }
    }

    public void HealthBarsToggle()
    {
        if (_healthBarsToggle.isOn)
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Health Bars", 1);
        }
        else
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Health Bars", 0);
        }
    }

    public void AlternateSFXToggle()
    {
        if (_alternateSFXToggle.isOn)
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "alternateSFXToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "alternateSFXToggle", 0);
        }
    }

    public void AlternateControlsToggle()
    {
        if (_alternateControlsToggle.isOn)
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Alternate_Controls", 1);
        }
        else
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Alternate_Controls", 0);
        }
    }

    public void SubtitlesToggle()
    {
        if (_subtitlesToggle.isOn)
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Subtitles", 1);
        }
        else
        {
            PlayerPrefs.SetInt(UserIdentifier + "_" + "Subtitles", 0);
        }
    }

    public void HealthBarsDisplayButton()
    {
        ToggleButton(_healthBarsToggle);
    }

    public void AltControlsButton()
    {
        ToggleButton(_alternateControlsToggle);
    }

    public void ClockButton()
    {
        ToggleButton(_clockToggle);
    }

    public void AltSFXButton()
    {
        ToggleButton(_alternateSFXToggle);
    }


    public void SubtitlesButton()
    {
        ToggleButton(_subtitlesToggle);
    }

    private void ToggleButton(Toggle toggle)
    {
        bool newToggleVal = toggle.isOn ? false : true;
        toggle.isOn = newToggleVal;
    }

    public void DoAchievementUnlock(string achievementId, System.Action<bool> onUnlock)
    {
        if (PlayGamesPlatform.Instance == null)
            return;

        PlayGamesPlatform.Instance.LoadAchievements((achievements) =>
        {
            bool alreadyUnlocked = false;
            if (achievements == null)
            {
                Debug.Log("achievements is NULL");
                return;
            }

            // Check if the achievement is already unlocked
            foreach (var achievement in achievements)
            {
                if (achievement.id == achievementId && achievement.completed)
                {
                    alreadyUnlocked = true;
                    break;
                }
            }

            // If the achievement is not already unlocked, unlock it
            if (!alreadyUnlocked)
            {
                Social.ReportProgress(achievementId, 100.0f, (bool success) =>
                {
                    onUnlock(success);
                });
            }
            else
            {
                onUnlock(false);
            }
        });
    }

    public void DoAchievementIncrement(string achievementId, int steps = 1)
    {
        if (PlayGamesPlatform.Instance == null)
            return;

        PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Achievement incremented: " + achievementId);
            }
            else
            {
                Debug.LogWarning("Failed to increment achievement: " + achievementId);
            }
        });
    }

    IEnumerator FadeMusicOut()
    {
        float fadeDuration = 2.5f; // Duration of the fade-out in seconds
        float startVolume = _music.volume; // Initial volume of the music

        while (_music.volume > 0)
        {
            // Calculate the new volume based on the elapsed time and fade duration
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                _music.volume = Mathf.Lerp(startVolume, 0f, t);
                yield return null;
            }

            // Ensure the volume is set to 0 when the fade-out is complete
            _music.volume = 0f;
        }

        // Optionally stop the music or perform other actions once the fade-out is complete
        _music.Stop();
    }

    public void BossMode()
    {
        PlayerPrefs.SetInt("Boss Mode", 1);
        if (_pressedStart)
            return;

        _pressedStart = true;
        _options.SetActive(false);
        _startButtonSFX.Play();
        _anim.SetTrigger("Start");
        StartCoroutine(FadeMusicOut());
    }
}
