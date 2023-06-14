using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx;
    [SerializeField]
    private AudioSource _sfxAlternate;
    [SerializeField]
    private Text _unlockMessage;


    private void Start()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "BeatTheGameCount", 0) == 1 && GameManager.Instance.IsLoggedIn && !GameManager.Instance.BossMode)
            _unlockMessage.gameObject.SetActive(true);
        else
            _unlockMessage.gameObject.SetActive(false);

        _sfx.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 1f);
    }

    public IEnumerator PlayAgainCoRoutine()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        // Check if the player has beaten the game three times
        int beatTheGameCount = PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "BeatTheGameCount", 0);
        if (beatTheGameCount == 3 && !GameManager.Instance.BossMode)
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

        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Game");
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

    public void PlayAgainButton()
    {
        StartCoroutine(PlayAgainCoRoutine());
    }

    public void QuitButton()
    {
        StartCoroutine(QuitCoRoutine());
    }
}
