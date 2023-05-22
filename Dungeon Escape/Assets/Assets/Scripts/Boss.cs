using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private Player _player;
    public Slider Slider { get; set; }
    public int Health { get; set; }
    private bool isDead = false;
    private bool _metPlayer = false;
    private float elapsedTime = 0f;
    private float interval = 5f;
    private List<string> attackTriggers = new List<string> { "Attack1", "Attack2", "Attack3" };

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("collider.name: " + collider.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Health = 5;

        // Initialize the elapsed time to the current time
        elapsedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_metPlayer && Vector3.Distance(transform.position, _player.transform.position) < 3)
        {
            AudioManager.Instance.PlayKingLaughSound();
            _metPlayer = true;
        }
        // Calculate the time difference between the current time and the start time
        float timeSinceStart = Time.time - elapsedTime;

        // Check if 30 seconds have passed
        if (timeSinceStart >= interval)
        {
            // Call a function or perform a random attack every 30 seconds
            DoRandomAttack();

            // Reset the elapsed time to start counting again
            elapsedTime = Time.time;
        }

        if(!isDead)
        {
            Movement();
        }
    }


    public virtual void Movement()
    {
        UpdateFlip();
    }

    public void Damage()
    {

        //if (Health > 1 && !GameManager.Instance.GotKonamiCode)
        if (Health > 1)
            AudioManager.Instance.PlayKingHitSound();
        else
            AudioManager.Instance.PlayKingDeathSound();
        Health--;

        _anim.SetTrigger("Hit");

        if (Health == 0)
        {
            if (Slider != null)
                Slider.gameObject.SetActive(false);
            isDead = true;
            _anim.SetTrigger("Death");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;

            // Kill all enemeis achivement
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
        }
    }

    private void DoRandomAttack()
    {
        float comboChance = Random.Range(0f, 1f);
        Debug.Log("comboChance: " + comboChance);
        if (comboChance > 0.5f)
            StartCoroutine(ComboCoolDown());
        else
        {
            string randomTrigger = attackTriggers[Random.Range(0, attackTriggers.Count)];
            Debug.Log("randomTrigger: " + randomTrigger);
            _anim.SetTrigger(randomTrigger);
        }
    }

    private void UpdateFlip()
    {
        Vector3 direction = _player.transform.localPosition - transform.localPosition;
        if (direction.x < 0)
            _spriteRenderer.flipX = true;
        else if (direction.x > 0)
            _spriteRenderer.flipX = false;
    }

    IEnumerator ComboCoolDown()
    {
        _anim.SetBool("Combo", true);
        _anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(5);
        _anim.SetBool("Combo", false);
    }
}
