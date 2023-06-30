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

  
    private void Start()
    {
        _sfx.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
    }

    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            //Debug.Log("Authenticated. Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
        }
        else
        {
            //Debug.Log("*** Failed to authenticate with " + signInStatus);
        }
    }

    IEnumerator TryAgainCoRoutine()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
        {
            _sfx.Play();
        }
        yield return new WaitForSeconds(0.4f);

        try
        {
            if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "DeathCount", 0) >= 5 && !GameManager.Instance.BossMode)
            {
                //Debug.Log("You died more than 5 times and didn't give up!");
                GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_persistent, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_still_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_completionist);
                    }
                });
            }
        }
        catch (Exception)
        {
            //Debug.LogError("There was an error regarding checking for number of deaths");
        }
        finally
        {
            if (GameManager.Instance.BossMode)
                PlayerPrefs.SetInt("Boss Mode", 1);
            SceneManager.LoadScene("Game");
        }
    }

    IEnumerator QuitCoRoutine()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
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
