using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : StateMachineBehaviour
{
    private float timer = 0f;
    private float loopDuration = 0.6f;
    [SerializeField]
    private Boss _boss;
    private Player _player;
    [SerializeField]
    private Transform _pointA;
    [SerializeField]
    private Transform _pointB;
    [SerializeField]
    private Vector3 _randomPoint;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _hitBox;
    [SerializeField]
    private SpriteRenderer _shield;
    private bool foundRandomPoint;
    [SerializeField]
    private SpriteRenderer _TeleportEffectPrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("GoingRight", false);
        animator.SetBool("GoingLeft", false);
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _pointA = GameObject.Find("Boss_Point_A").GetComponent<Transform>();
        _pointB = GameObject.Find("Boss_Point_B").GetComponent<Transform>();
        _spriteRenderer = animator.GetComponent<SpriteRenderer>();
        _hitBox = animator.GetComponentInChildren<BoxCollider2D>();
        timer = 0f;
        foundRandomPoint = false;


        while (!foundRandomPoint)
        {
            float randomXPos = Random.Range(_pointA.position.x, _pointB.position.x);
            Vector3 randomPos = new Vector3(randomXPos, _boss.transform.position.y, _boss.transform.position.z);
            if (Vector3.Distance(randomPos, _player.transform.position) > 1) // && Vector3.Distance(randomPos, _player.transform.position) < 6)
            {
                _randomPoint = randomPos;
                foundRandomPoint = true;
            }
        }

        _TeleportEffectPrefab.transform.localScale = new Vector3(3, 3, 3);
        Vector3 teleportTo = new Vector3(_boss.transform.position.x, -8.474f, _boss.transform.position.z);
        SpriteRenderer teleportEffect = Instantiate(_TeleportEffectPrefab, teleportTo, Quaternion.identity);
        AudioManager.Instance.PlayTeleportSFX();
        _boss.transform.position = _randomPoint;
        Destroy(teleportEffect.gameObject, 0.417f);

        UpdateFlip();

        if (_spriteRenderer.flipX)
        {
            animator.SetBool("GoingLeft", true);
            animator.SetBool("GoingRight", false);
        }
        else
        {
            animator.SetBool("GoingLeft", false);
            animator.SetBool("GoingRight", true);
        }

        _shield = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
        _boss.ShieldOn = false;
        _shield.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer >= loopDuration)
        {
            animator.SetTrigger("Attack1");
            AudioManager.Instance.PlayKingAttack1SFX();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack1");
    }

    private void UpdateFlip()
    {
        Vector3 direction = _player.transform.localPosition - _boss.transform.localPosition;
        if (direction.x < 0)
            _spriteRenderer.flipX = true;
        else if (direction.x > 0)
            _spriteRenderer.flipX = false;
    }
}