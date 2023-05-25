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
    private bool isDead = false;
    private bool _metPlayer = false;
    private float elapsedTime = 0f;
    private float interval = 5f;
    private float _speed = 6f;
    //private float _jumpForce = 6.5f;
    private float _jumpForce = 10f;
    private float _prevVelocityY = 0;
    private bool _grounded = true;
    private bool _resetJump = false;
    private bool _move = false;
    private int reachPointCount = 0;


    private List<string> attackTriggers = new List<string> { "Attack1", "Attack2", "Attack3" };

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("collider.name: " + collider.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Health = 5;
        CurrentTarget = _pointB;

        // Initialize the elapsed time to the current time
        elapsedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_rigidBody.velocity.y < _prevVelocityY && !IsGrounded())
            _anim.SetBool("Fall", true);

        _prevVelocityY = _rigidBody.velocity.y;
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
        */
    }


    public virtual void Movement()
    {
        Debug.Log("In Movement, reachPointCount: " + reachPointCount);
        if (reachPointCount == 2)
        {
            //Jump();
            Jump();
        }

        if (Vector3.Distance(transform.position, _pointA.position) < 1.5f)
        {
            Jump();
            CurrentTarget = _pointB;
            reachPointCount++;
        }
        else if (Vector3.Distance(transform.position, _pointB.position) < 1.5f)
        {
            Jump();
            CurrentTarget = _pointA;
            reachPointCount++;
        }

        _grounded = IsGrounded();
        _anim.SetBool("Grounded", _grounded);
        UpdateFlip();
        if (CurrentTarget == _pointB)
        {
            MoveRight();
        }
        else if (CurrentTarget == _pointA)
        {
            MoveLeft();
        }

        _anim.SetBool("Move", _move);
        

    }

    private void MoveLeft()
    {
        _spriteRenderer.flipX = true;
        _move = true;
        Debug.Log("in MoveLeft");
        _anim.SetBool("Move", true);
        _rigidBody.velocity = new Vector2(-1 * _speed, _rigidBody.velocity.y);
    }

    private void MoveRight()
    {
        _spriteRenderer.flipX = false;
        _move = true;
        _anim.SetTrigger("Run");
        _anim.SetBool("Move", true);
        _rigidBody.velocity = new Vector2(1 * _speed, _rigidBody.velocity.y);
    }

    private void Jump()
    {
        if (!IsGrounded())
            return;

        StartCoroutine(ResetJumpRoutine());
        _anim.SetBool("Jumping", true);
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
    }

    public void Damage()
    {
        if (Health <= 0)
            return;

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

            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //gameObject.GetComponent<Rigidbody2D>().simulated = false;
            
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

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

    private bool IsGrounded()
    {
        Vector3 pos = transform.position;
        //pos.y -= 10;
        Debug.DrawRay(pos, Vector2.down * 0.85f, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        RaycastHit2D hitInfo = Physics2D.Raycast(pos, Vector2.down, 0.85f, 1 << 8);
        if (hitInfo.collider != null)
        {
            if (!_resetJump)
            {
                _anim.SetBool("Jumping", false);
                _anim.SetBool("Fall", false);
                return true;
            }
        }
        return false;
    }

    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }

    private void JumpOrIdle()
    {
        int shouldJump = Random.Range(0, 2);
        if (shouldJump == 1)
            Jump();
        else if(IsGrounded())
            _anim.SetTrigger("Idle");
    }
}
