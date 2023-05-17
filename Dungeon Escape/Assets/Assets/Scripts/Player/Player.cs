using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private float _jumpForce = 6.5f;
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private LayerMask _groundLayer;
    private bool _resetJump = false;
    private bool _grounded = false;
    private PlayerAnimation _playerAnimation;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _arcSpriteRenderer;
    [SerializeField]
    private int _numDiamonds;
    [SerializeField]
    private GameObject _shopPanel;
    public int Health { get; set; }
    [SerializeField]
    private GameObject _doorA;
    [SerializeField]
    private GameObject _doorB;
    private bool _canEnterDoor = true;
    private float arrowsHorizontal;
    private float arrowsVertical;
    

    public bool IsHit { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _arcSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        Health = 4;
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHit)
        {
            // Disable movement and collision detection
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            return;
        }
        else
        {
            // Enable movement and collision detection
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().enabled = true;
        }

        float moveUpDown;
        float move;
        Movement(out move, out moveUpDown);

        //if (_grounded && (CrossPlatformInputManager.GetButtonDown("A_Button")))
        if (CrossPlatformInputManager.GetButtonDown("A_Button") || Input.GetKeyDown(KeyCode.S))
        {
            if (_grounded)
            {
                _playerAnimation.Attack();
            }
            else
            {
                _playerAnimation.JumpAttack();
            }
            AudioManager.Instance.PlayAttackSound();

            // Konami Code Related
            GameManager.Instance.KonamiCodeString += "A";
            GameManager.Instance.HandleKonamiCode();

        }

        if (GameManager.Instance.PlayerInFrontOfDoorA && moveUpDown > 0.85f && Mathf.Abs(move) < 0.4 && _canEnterDoor)
        {
            transform.position = _doorB.transform.position;
            GameManager.Instance.PlayerInFrontOfDoorA = false;
            GameManager.Instance.PlayerInFrontOfDoorB = true;
            StartCoroutine(EnterDoorCooldown());
        }
        else if (GameManager.Instance.PlayerInFrontOfDoorB && moveUpDown > 0.85f  && Mathf.Abs(move) < 0.4 && _canEnterDoor)
        {
            transform.position = _doorA.transform.position;
            GameManager.Instance.PlayerInFrontOfDoorB = false;
            GameManager.Instance.PlayerInFrontOfDoorA = true;
            StartCoroutine(EnterDoorCooldown());
        }
    }

    private void Movement(out float move, out float moveUpDown)
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "Alternate_Controls", 0) == 1)
        {
            move = arrowsHorizontal;
            moveUpDown = arrowsVertical;
        }
        else
        {
            move = CrossPlatformInputManager.GetAxis("Horizontal");
            moveUpDown = CrossPlatformInputManager.GetAxis("Vertical");
        }
        GameManager.Instance.HandleKonamiCodeDirections(move, moveUpDown);

        _grounded = IsGrounded();
        _playerAnimation.SetGrounded(_grounded);

        SetRunningDirection(move);
        _rigidBody.velocity = new Vector2(move * _speed, _rigidBody.velocity.y);
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("B_Button")) && IsGrounded())
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _playerAnimation.Jump(true);
            AudioManager.Instance.PlayJumpSound();
            GameManager.Instance.numJumps++;

            if (GameManager.Instance.numJumps == 35)
            {
                Debug.Log("HyperActive!");
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_hyperactive, (bool achievementUnlocked) =>
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
        _playerAnimation.Move(move);

        // Konami Code Related
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("B_Button")))
        {
            GameManager.Instance.KonamiCodeString += "B";
            GameManager.Instance.HandleKonamiCode();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, 1 << 8);
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if(hitInfo.collider != null)
        {
            if (!_resetJump)
            {
                _playerAnimation.Jump(false);
                return true;
            }
        }
        return false;
    }

    void SetRunningDirection(float move)
    {
        if(move > 0)
        {
            _spriteRenderer.flipX = false;
            _arcSpriteRenderer.flipX = false;
            _arcSpriteRenderer.flipY = false;

            Vector3 newPos = _arcSpriteRenderer.transform.localPosition;
            newPos.x = 1.01f;
            _arcSpriteRenderer.transform.localPosition = newPos;
        }
        else if(move < 0)
        {
            _spriteRenderer.flipX = true;
            _arcSpriteRenderer.flipX = true;
            _arcSpriteRenderer.flipY = true;

            Vector3 newPos = _arcSpriteRenderer.transform.localPosition;
            newPos.x = -1.01f;
            _arcSpriteRenderer.transform.localPosition = newPos;
        }
    }

    IEnumerator ResetJumpRoutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }

    public void OnUpButtonDown()
    {
        arrowsHorizontal = 0;
        arrowsVertical = 1;
    }

    /*
    public void OnUpButtonEnter()
    {
        arrowsHorizontal = 0;
        arrowsVertical = 1;
    }
    */

    public void OnUpButtonUp()
    {
        arrowsHorizontal = 0;
        arrowsVertical = 0;
    }

    public void OnUpButtonExit()
    {
        arrowsHorizontal = 0;
        arrowsVertical = 0;
    }

    public void OnRightButtonDown()
    {
        arrowsHorizontal = 1f;
        arrowsVertical = 0f;
    }

    public void OnRightButtonEnter()
    {
        if (GameManager.Instance.DuringKonamiCode)
            return;

        arrowsHorizontal = 1f;
        arrowsVertical = 0f;
    }

    /*
    public void OnRightButtonUp()
    {
        
        // Stop moving the player
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
        
    }
    */

    public void OnRightButtonExit()
    {

        // Stop moving the player
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;

    }

    public void OnDownButtonDown()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = -1f;
    }

    /*
    public void OnDownButtonEnter()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = -1f;
    }
    */

    public void OnDownButtonUp()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
    }

    public void OnDownButtonExit()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
    }

    public void OnLeftButtonDown()
    {
        arrowsHorizontal = -1f;
        arrowsVertical = 0f;
    }

    public void OnLeftButtonEnter()
    {
        if (GameManager.Instance.DuringKonamiCode)
            return;

        arrowsHorizontal = -1f;
        arrowsVertical = 0f;
    }

    /*
    public void OnLeftButtonUp()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
    }
    */

    public void OnLeftButtonExit()
    {
        arrowsHorizontal = 0f;
        arrowsVertical = 0f;
    }

    public void Damage()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();

        if (stackFrames.Length >= 2)
        {
            System.Diagnostics.StackFrame callingFrame = stackFrames[1];
            string callingMethodName = callingFrame.GetMethod().Name;
            UnityEngine.Debug.Log("Called by: " + callingMethodName);
        }
        //if (IsHit)
        //  return;
        if (Health < 0 || GameManager.Instance.GotKonamiCode)
        {
            return;
        }
        else if (Health > 1)
        {
            AudioManager.Instance.PlayGettingHitSound();
            //StartCoroutine(PlayerHitCoolDown());
            _playerAnimation.Hit();
        }

        Health--;
        GameManager.Instance.NoHitRun = false;

        UIManager.Instance.UpdateLives(Health);
        if (Health == 0)
        {
            GameManager.Instance.IsDead = true;
            AudioManager.Instance.PlayPlayerDeathSFX();
            OnDeath();
        }
    }

    public int getNumDiamonds()
    {
        return _numDiamonds;
    }

    public void setNumDiamonds(int numDiamonds)
    {
        _numDiamonds = numDiamonds;
        UIManager.Instance.UpdateGemCount(_numDiamonds);
    }

    public void AddGems(int amount, bool fromShop = false)
    {
        _numDiamonds += amount;
        UIManager.Instance.UpdateGemCount(_numDiamonds);

        if (!fromShop)
        {
            GameManager.Instance.numDiamondsCollected += amount;
            if (GameManager.Instance.numDiamondsCollected == GameManager.Instance.NumDiamondsInGame) //250)
            {
                Debug.Log("Collected all diamonds!"); // Achievement 3
                GameManager.Instance.DoAchievementUnlock(SmokeTest.GPGSIds.achievement_gem_hunter, (bool achievementUnlocked) =>
                {
                    if (achievementUnlocked)
                    {
                    // The achievement was unlocked, so increment the Completionist achievement
                    GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_half_way_there);
                        GameManager.Instance.DoAchievementIncrement(SmokeTest.GPGSIds.achievement_completionist);
                    }
                });
                GameManager.Instance.CollectedAllDiamonds = true;
            }
        }
    }

    public void HitSpike()
    {
        Health = 0;
        AudioManager.Instance.PlayImpaleSound();
        UIManager.Instance.UpdateLivesOnSuddenDeath();
        OnDeath();
    }

    public void OnDeath()
    {
        AudioManager.Instance.PlayGameOverMusic();
        gameObject.GetComponent<Player>().enabled = false;
        _playerAnimation.Death();
        UIManager.Instance.StartFadeOut("Game_Over");
        PlayerPrefs.SetInt(GameManager.Instance.UserIdentifier + "_" + "DeathCount", PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "DeathCount", 0) + 1);
    }

    public void WearBootsOfFlight()
    {
        _jumpForce += 2.5f;
        if (GameManager.Instance.GotKonamiCode)
            _speed += 1f;
        else
            _speed += 4f;
    }

    public void KonamiCodeSpeed()
    {
        if(!GameManager.Instance.HasBootsOfFlight)
            _speed += 3f;
    }

    /*
    IEnumerator PlayerHitCoolDown()
    {
        IsHit = true;
        yield return new WaitForSeconds(1f);
        IsHit = false;
    }
    */

    public void AnimationStart()
    {
        // Called by the animation event when it starts
        Debug.Log("AnimationStart in Player");
        IsHit = true;
    }

    public void AnimationEnd()
    {
        // Called by the animation event when it ends
        Debug.Log("AnimationEnd in Player");
        IsHit = false;
    }

    private IEnumerator EnterDoorCooldown()
    {
        _canEnterDoor = false;
        yield return new WaitForSeconds(0.6f);
        _canEnterDoor = true;
    }

    public PlayerAnimation GetPlayerAnimation()
    {
        return _playerAnimation;
    }
}
