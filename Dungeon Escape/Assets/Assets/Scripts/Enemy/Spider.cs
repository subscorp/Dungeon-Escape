using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    [SerializeField]
    private AcidEffect _acidEffectPrefab;
    [SerializeField]
    private GameObject _diamondPrefabUnderground;
    [SerializeField]
    private GameObject _chestPrefab;
    private Vector3 _chestPosition = new Vector3(-15.3f, -22.291f, 0); // Scale = (3, 3, 1)
    public int Health { get; set; }

    public void Damage()
    {
        AudioManager.Instance.PlaySpiderHitSound();
        Health--;
        isHit = true;

        animator.SetBool("InCombat", true);
        animator.SetTrigger("Hit");


        if (Health == 0)
        {
            if (Slider != null)
                Slider.gameObject.SetActive(false);
            isDead = true;
            animator.SetTrigger("Death");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            GameObject diamondGameObject;
            if (_diamondPrefabUnderground != null)
                diamondGameObject = Instantiate(_diamondPrefabUnderground, transform.position, Quaternion.identity);
            else
                diamondGameObject = Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
            Diamond diamond = diamondGameObject.GetComponent<Diamond>();
            diamond.SetVal(gems);
            diamond.SetScale(1.45f);
            GameManager.Instance.numEnemiesKilled += 1;
            if (GameManager.Instance.numEnemiesKilled == GameManager.Instance.NumEnemiesInGAme)
            {
                Debug.Log("Killed all enemies!"); // Achivement 2
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_fighter, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                        // The achievement was unlocked, so increment the Completionist achievement
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_half_way_there);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
            }

            if(transform.name == "Spider_Enemy_2")
                GameManager.Instance.FirstCaveSpiderDead = true;
            else if(transform.name == "Spider_Enemy_3")
                GameManager.Instance.SecondCaveSpiderDead = true;

            if(GameManager.Instance.FirstCaveSpiderDead && GameManager.Instance.SecondCaveSpiderDead && !GameManager.Instance.InstantiatedChest)
            {
                StartCoroutine(SpawnChestAfterDelay());
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
        // Sits still
    }

    public void Attack()
    {
        Instantiate(_acidEffectPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator SpawnChestAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(_chestPrefab, _chestPosition, Quaternion.identity);
        AudioManager.Instance.PlayChestAppearsSFX();
        GameManager.Instance.InstantiatedChest = true;
    }
}
