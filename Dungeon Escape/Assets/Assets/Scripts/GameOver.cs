using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx;
    [SerializeField]
    private AudioSource _sfxAlternate;
    public string UserIdentifier { get; set; }

    private void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
        UserIdentifier = Social.localUser.userName;
        if (UserIdentifier == null || UserIdentifier == "")
            UserIdentifier = "temp";

        _sfx.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 0.6f);
    }

    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Authenticated. Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
        }
        else
        {
            Debug.Log("*** Failed to authenticate with " + signInStatus);
        }
    }

    IEnumerator TryAgainCoRoutine()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
        {
            _sfx.Play();
        }
        yield return new WaitForSeconds(0.4f);

        try
        {
            if (PlayerPrefs.GetInt(UserIdentifier + "_" + "DeathCount", 0) >= 5)
            {
                Debug.Log("You died more than 5 times and didn't give up!");
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_persistent, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_half_way_there);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
            }
        }
        catch (Exception)
        {
            Debug.LogError("There was an error regarding checking for number of deaths");
        }
        finally
        {
            SceneManager.LoadScene("Game");
        }
    }

    IEnumerator QuitCoRoutine()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("Main_Menu");
    }

    public void TryAgainButton()
    {
        StartCoroutine(TryAgainCoRoutine());
    }

    public void QuitButton()
    {
        StartCoroutine(QuitCoRoutine());
    }
}
