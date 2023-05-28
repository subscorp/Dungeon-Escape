using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePhaseOne : StateMachineBehaviour
{
    private float timer = 0f;
    private float loopDuration = 1f;
    [SerializeField]
    private Boss _boss;
    [SerializeField]
    private Transform _pointA;
    [SerializeField]
    private Transform _pointB;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _hitBox;
    [SerializeField]
    private SpriteRenderer _shield;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("GoingRight", false);
        animator.SetBool("GoingLeft", false);
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        _pointA = GameObject.Find("Boss_Point_A").GetComponent<Transform>();
        _pointB = GameObject.Find("Boss_Point_B").GetComponent<Transform>();
        _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        _spriteRenderer = animator.GetComponent<SpriteRenderer>();
        _hitBox = animator.GetComponentInChildren<BoxCollider2D>();

        _rigidBody.velocity = new Vector2(0, 0);
        timer = 0f;
        float distToPointA = Vector3.Distance(_boss.transform.position, _pointA.position);
        float distToPointB = Vector3.Distance(_boss.transform.position, _pointB.position);

        if (distToPointA < distToPointB)
        {
            _spriteRenderer.flipX = false;
            animator.SetBool("GoingRight", true);
        }
        else
        {
            _spriteRenderer.flipX = true;
            animator.SetBool("GoingLeft", true);
        }

        _shield = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
        _boss.ShieldOn = false;
        _shield.enabled = false;
        if (!GameManager.Instance.StartedBossFight)
        {
            AudioManager.Instance.PlayKingLaughSound();
            AudioManager.Instance.PlayBossMusic();
            GameManager.Instance.StartedBossFight = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer >= loopDuration)
        {
            // Transition to another state or perform actions
            // when the loop duration is reached
            animator.SetTrigger("Attack1");
            AudioManager.Instance.PlayKingAttack1SFX();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack1");
    }
}
