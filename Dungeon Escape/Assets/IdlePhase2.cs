using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePhase2 : StateMachineBehaviour
{
    private float timer = 0f;
    private float loopDuration = 1.2f;
    [SerializeField]
    private Boss _boss;
    [SerializeField]
    private Transform _pointA;
    [SerializeField]
    private Transform _pointB;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _hitBox;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("GoingRight", false);
        animator.SetBool("GoingLeft", false);
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
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
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer >= loopDuration)
        {
            // Transition to another state or perform actions
            // when the loop duration is reached
            animator.SetTrigger("Run");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Run");
    }
}
