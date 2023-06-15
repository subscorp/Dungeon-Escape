using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private Text _BeatTime;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != "Player")
            return;

        if (GameManager.Instance.HasKeyToCastle)
        {
            Debug.Log("Beat the game!"); // Achievement 1

            AudioManager.Instance.PlayWinMusic();

            PlayerPrefs.SetInt(GameManager.Instance.UserIdentifier + "_" + "BeatTheGameCount", PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "BeatTheGameCount", 0) + 1);
            
            GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_escaped_the_dungeon, (bool achievementUnlocked) =>
            {
                if (achievementUnlocked)
                {
                    // The achievement was unlocked, so increment the Completionist achievement
                    GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_still_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                }
            });

            if(GameManager.Instance.NoHitRun && !GameManager.Instance.GotKonamiCode)
            {
                Debug.Log("You beat the game without getting hit!"); // Achievement 4
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_no_hit_run, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_still_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
            }
            if(GameManager.Instance.DidNotWatchAd)
            {
                Debug.Log("Consumer friendly"); // Achievement 5
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_consumer_friendly, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_still_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
            }

            Debug.Log("Beat the game in: " + GameManager.Instance.ElapsedTime);
            long score = (long)Mathf.RoundToInt(GameManager.Instance.ElapsedTime * 1000f);

            // Get the number of whole minutes
            int minutes = (int)(GameManager.Instance.ElapsedTime / 60);

            // Get the number of remaining seconds
            float seconds = GameManager.Instance.ElapsedTime % 60;

            // Format the time as a string in the format "MM:SS"
            GameManager.Instance.CurrentBeatTime = string.Format("{0:00}:{1:00.00}", minutes, seconds);
            GameManager.Instance.GameComplete = true;

            Debug.Log("Which is: " + GameManager.Instance.CurrentBeatTime);
            _BeatTime.text = "Time: " + GameManager.Instance.CurrentBeatTime;
            if (GameManager.Instance.ElapsedTime < 100f)
            {
                Debug.Log("Beat the game in under 1:40"); // Achievement 6
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_speed_runner, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_still_on_track_to_completion);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
            }
            GameManager.Instance.SubmitScore(SmokeTest.GPGSIds.leaderboard_hall_of_fame, score, null);
            UIManager.Instance.StartFadeOut("Win");
        }
        else
        {
            bool needKeyToChest = false;
            if (GameManager.Instance.HasKeyToChest)
                UIManager.Instance.EnableWrongKeyPanel();
            else
                UIManager.Instance.EnableNeedKeyToCastlePanel();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name != "Player")
            return;

        UIManager.Instance.DisableNeedKeyToCastlePanel();
        UIManager.Instance.DisableWrongKeyPanel();
    }
}
