using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscapesTrigger : MonoBehaviour
{
    private Animator _bossAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.BossDead)
        {
            GameObject _bossObject = GameObject.FindGameObjectWithTag("Boss");
            if (_bossObject != null)
            {
                _bossAnimator = _bossObject.GetComponentInChildren<Animator>();
                _bossAnimator.SetTrigger("Player_Escapes");

                // Nope
                Debug.Log("Nope");
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_nope, (bool achievementUnlocked) =>
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
            GameManager.Instance.StartedBossFight = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.StartedBossFight = false;
    }
}
