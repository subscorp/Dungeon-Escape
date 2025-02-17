using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Player _player;
    [SerializeField]
    private Transform _pointA, _pointB;
    public Transform CurrentTarget { get; set; }
    public Slider Slider { get; set; }
    public int Health { get; set; }
    [SerializeField]
    protected int maxHealth;
    public bool ShieldOn { get; set; }
    private Animator _gateAnim;

    [SerializeField]
    private float slowdownDuration = 10f;
    [SerializeField]
    private float _startTimeScaleValue = 0.1f;
    private float _endTimeScaleValue = 1f;

    private float originalTimeScale;
    private Color _originalColor;
    protected float _currentVelocity = 0;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _gateAnim = GameObject.Find("Gate").GetComponent<Animator>();
        Slider = GetComponentInChildren<Slider>();
        Health = 9;
        CurrentTarget = _pointB;
        ShieldOn = false;
        _originalColor = _spriteRenderer.color;
        maxHealth = 9;
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "Health Bars", 0) == 1)
            Slider.gameObject.SetActive(true);
        else
            Slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.UpdateEnemyLifeBar(Slider, (float)Health / (float)maxHealth, _currentVelocity);

        if (Health == 9)
        {
            _spriteRenderer.color = _originalColor;
        }
        else if (Health == 6)
        {
            _anim.SetBool("Phase1", false);
            _anim.SetBool("Phase2", true);
        }
        else if(Health == 3)
        {
            _anim.SetBool("Phase2", false);
            _anim.SetBool("Phase3", true);
            _spriteRenderer.color = Color.red;
        }
    }

    public void Damage()
    {
        if (Health <= 0 || ShieldOn)
            return;

        //if (Health > 1 && !GameManager.Instance.GotKonamiCode)
        if (Health > 1 && Health != 4)
            AudioManager.Instance.PlayKingHitSound();
        else if (Health == 4)
            AudioManager.Instance.PlayKingRageSFX();
        else
            AudioManager.Instance.PlayKingDeathSound();
        Health--;

        _anim.SetTrigger("Hit");

        if (Health == 0)
        {
            GameManager.Instance.BossDead = true;

            if (Slider != null)
                Slider.gameObject.SetActive(false);

            _anim.SetTrigger("Death");

            Time.timeScale = _startTimeScaleValue;
            StartSlowMotion();
            //StartCoroutine(SlowMotionOnKingDeathRoutine());

            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            // Kill all enemeis achivement
            GameManager.Instance.numEnemiesKilled += 1;
            if (GameManager.Instance.numEnemiesKilled == GameManager.Instance.NumEnemiesInGAme)
            {
                //Debug.Log("Killed all enemies!"); // Achivement 2
                GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_fighter, (bool achievementUnlocked) =>
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

            if(GameManager.Instance.BossMode)
            {
                GameManager.Instance.HandleWinOnBossMode();
            }
            else
                AudioManager.Instance.FadeOutBossMusic();
        }
    }

    // Call this method when you want to trigger the slow-motion effect
    public void StartSlowMotion()
    {
        StartCoroutine(SlowMotionCoroutine());   
    }

    private IEnumerator SlowMotionCoroutine()
    {
        // Store the original Time.timeScale value
        //originalTimeScale = Time.timeScale;
        originalTimeScale = Time.unscaledDeltaTime;
        //Debug.Log("originalTimeScale (shuold be 0.4f): " + originalTimeScale);

        // Gradually reduce Time.timeScale over the specified duration
        float currentTime = 0f;
        while (currentTime < slowdownDuration)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / slowdownDuration);
            Time.timeScale = Mathf.Lerp(_startTimeScaleValue, _endTimeScaleValue, t);
            yield return null;
        }

        // Ensure that Time.timeScale is set to the desired value
        Time.timeScale = _endTimeScaleValue;
        //Debug.Log("Time.timeScale at end (should be 1): " + Time.timeScale);
        _gateAnim.SetTrigger("Gate_Open");
        AudioManager.Instance.PlayCastleGateOpen();
    }    
}

