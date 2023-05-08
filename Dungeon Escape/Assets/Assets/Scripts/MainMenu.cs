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

    public string UserIdentifier { get; set; }


    private void Awake()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
        _options.SetActive(false);
    }

    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Authenticated. Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
            UserIdentifier = Social.localUser.userName;
            if (UserIdentifier == null || UserIdentifier == "")
                UserIdentifier = "temp";
            
            Debug.Log("In MainMenu::Start, UserIdentifier: " + UserIdentifier);
            _music.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "Music", 0.6f);
            _sfx.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
            _sfxAlternate.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);

            musicSliderVal.value = PlayerPrefs.GetFloat(UserIdentifier + "_" + "Music", 0.6f);
            SFXSliderVal.value = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);

            Debug.Log("In MainMenu::Start, PlayerPrefs.GetInt(UserIdentifier + '_' + 'BeatTheGameCount', 0): " + PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0));

            if (PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0) >= 1)
            {
                Debug.Log("In beat the game >= 1");
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
            }
            else
            {
                _clockToggle.isOn = false;
                _alternateSFXToggle.isOn = false;
                _alternateControlsToggle.isOn = false;
                _clockText.gameObject.SetActive(false);
                _altSFXText.gameObject.SetActive(false);
                _altControlsText.gameObject.SetActive(false);
                _clockToggle.gameObject.SetActive(false);
                _alternateSFXToggle.gameObject.SetActive(false);
                _alternateControlsToggle.gameObject.SetActive(false);
            }

        }
        else
        {
            UserIdentifier = "temp";
            Debug.Log("*** Failed to authenticate with " + signInStatus);
            _clockToggle.isOn = false;
            _alternateSFXToggle.isOn = false;
            _alternateControlsToggle.isOn = false;
            _clockText.gameObject.SetActive(false);
            _altSFXText.gameObject.SetActive(false);
            _altControlsText.gameObject.SetActive(false);
            _clockToggle.gameObject.SetActive(false);
            _alternateSFXToggle.gameObject.SetActive(false);
            _alternateControlsToggle.gameObject.SetActive(false);

            _music.volume = 0.6f; 
            _sfx.volume = 0.6f;  
            _sfxAlternate.volume = 0.6f;
            musicSliderVal.value = 0.6f; 
            SFXSliderVal.value = 0.6f;
        }
    }

    public void StartButton()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        try
        {
            // Check if the player has beaten the game three times
            int beatTheGameCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0);
            if (beatTheGameCount == 3)
            {
                // Try to unlock the "Addicted" achievement
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_addicted, (bool achievementUnlocked) =>
                {
                    
                    if (achievementUnlocked)
                    {
                        // Increment the "Completionist" achievement if "Addicted" was unlocked
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_half_way_there);
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
            SceneManager.LoadScene("Game");
        }
    }

    public void QuitButton()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        Application.Quit();
    }

    public void MenuButton()
    {
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
        Debug.Log("BackButton");
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
        Debug.Log("Music val: " + val);
        PlayerPrefs.SetFloat(UserIdentifier + "_" + "Music", val);
        Debug.Log("in MainMenu::MusicSlider");
        Debug.Log("UserIdentifier: " + UserIdentifier);
        Debug.Log("PlayerPrefs.GetFloat(UserIdentifier + '_' + 'Music', 0.6f);" + PlayerPrefs.GetFloat(UserIdentifier + "_" + "Music", 0.6f));
    }

    public void SFXSlider()
    {
        float val = SFXSliderVal.value;
        _sfx.volume = val;
        _sfxAlternate.volume = val;
        Debug.Log("SFX val: " + val);
        PlayerPrefs.SetFloat(UserIdentifier + "_" + "SFX", val);
        Debug.Log("in MainMenu::SFXSlider");
        Debug.Log("UserIdentifier: " + UserIdentifier);
        Debug.Log("PlayerPrefs.GetFloat(UserIdentifier + '_' + 'SFX', 0.6f);" + PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f));
    }

    public void RestoreToDefaultsButton()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        musicSliderVal.value = 0.6f;
        SFXSliderVal.value = 0.6f;
        _music.volume = 0.6f;
        _sfx.volume = 0.6f;
        _sfxAlternate.volume = 0.6f;
        int beatTheGameCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0);
        int deathCount = PlayerPrefs.GetInt(UserIdentifier + "_" + "DeathCount", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(UserIdentifier + "_" + "BeatTheGameCount", beatTheGameCount);
        PlayerPrefs.SetInt(UserIdentifier + "_" + "DeathCount", deathCount);
        PlayerPrefs.SetInt(UserIdentifier + "_" + "Clock", 0);
        _clockToggle.isOn = false;
        _alternateSFXToggle.isOn = false;
        _alternateControlsToggle.isOn = false;
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
}
