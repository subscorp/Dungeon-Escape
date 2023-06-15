using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy, IDamageable
{
    public int Health { get; set; }

    public void Damage()
    {
        if(Health > 1 && !GameManager.Instance.GotKonamiCode)
            AudioManager.Instance.PlaySkeletonHitSound();
        else
            AudioManager.Instance.PlaySkeletonDeathSFX();
        Health--;
        isHit = true;

        //animator.SetBool("InCombat", true);
        animator.SetTrigger("Hit");


        if (Health == 0 || GameManager.Instance.GotKonamiCode)
        {
            if (Slider != null)
                Slider.gameObject.SetActive(false);
            isDead = true;
            animator.SetTrigger("Death");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameObject diamondGameObject = Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
            Diamond diamond = diamondGameObject.GetComponent<Diamond>();
            diamond.SetVal(gems);
            diamond.SetScale(1.25f);
            GameManager.Instance.numEnemiesKilled += 1;
            if (GameManager.Instance.numEnemiesKilled == GameManager.Instance.NumEnemiesInGAme)
            {
                Debug.Log("Killed all enemies!"); // Achivement 2
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_fighter, (bool achievementUnlocked) =>
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

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public override void Update()
    {
        base.Update();
        UIManager.Instance.UpdateEnemyLifeBar(Slider, (float)Health / (float)maxHealth, _currentVelocity);
    }

    public override void Movement()
    {
        base.Movement();
    }
}
