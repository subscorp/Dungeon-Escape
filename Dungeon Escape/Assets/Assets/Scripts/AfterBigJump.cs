using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBigJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.Instance.DidBigJump)
            return;

        if (collider.name == "Player")
        {
            if (!GameManager.Instance.HasBootsOfFlight && !GameManager.Instance.BossMode)
            {
                GameManager.Instance.DidBigJump = true;
                Debug.Log("But It Is Possible achievement");
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_but_it_is_possible, (bool achievementUnlocked) =>
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
        }
    }
}
