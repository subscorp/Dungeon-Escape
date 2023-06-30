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
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "BeatTheGameCount", 0) == 1 && !GameManager.Instance.BossMode)
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
        if (beatTheGameCount == 2 && !GameManager.Instance.BossMode)
        {
            // Try to unlock the "Addicted" achievement
            GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_addicted, (bool achievementUnlocked) =>
            {
                if (achievementUnlocked)
                {
                    // Increment the "Completionist" achievement if "Addicted" was unlocked
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_still_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_completionist);
                }
            });
        }

        yield return new WaitForSeconds(0.4f);

        if(GameManager.Instance.BossMode)
            PlayerPrefs.SetInt("Boss Mode", 1);

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
