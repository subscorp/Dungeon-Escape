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

    public string UserIdentifier { get; set; }

    private void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
        UserIdentifier = Social.localUser.userName;
        if (UserIdentifier == null || UserIdentifier == "")
            UserIdentifier = "temp";

        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "BeatTheGameCount", 0) == 1)
            _unlockMessage.gameObject.SetActive(true);
        else
            _unlockMessage.gameObject.SetActive(false);

        _sfx.volume = PlayerPrefs.GetFloat(UserIdentifier + "_" + "SFX", 1f);
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

    public IEnumerator PlayAgainCoRoutine()
    {
        if (PlayerPrefs.GetInt(UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

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

        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Game");
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

    public void PlayAgainButton()
    {
        StartCoroutine(PlayAgainCoRoutine());
    }

    public void QuitButton()
    {
        StartCoroutine(QuitCoRoutine());
    }
}
