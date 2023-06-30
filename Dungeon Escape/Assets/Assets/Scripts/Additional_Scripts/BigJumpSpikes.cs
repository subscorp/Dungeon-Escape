using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigJumpSpikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            if (!GameManager.Instance.HasBootsOfFlight && !GameManager.Instance.GotKonamiCode)
            {
                //Debug.Log("I Told You Not To Do This achievement");
                GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_i_told_you_not_to_do_this, (bool achievementUnlocked) =>
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
    }
}
