using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunLeft : StateMachineBehaviour
{
    private float _speed = 6f;
    private float _prevVelocityY;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    private Boss _boss;
    private Transform _pointA;
    private Transform _pointB;
    private Transform _pointM;
    [SerializeField]
    private SpriteRenderer _shield;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        _pointA = GameObject.Find("Boss_Point_A").GetComponent<Transform>();
        _pointB = GameObject.Find("Boss_Point_B").GetComponent<Transform>();
        _pointM = GameObject.Find("Boss_Point_M").GetComponent<Transform>();

        _shield = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
        _boss.ShieldOn = true;
        _shield.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(_boss.transform.position, _pointM.position) < 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (Vector3.Distance(_boss.transform.position, _pointA.position) < 2)
            animator.SetTrigger("Idle");

        _rigidBody.velocity = new Vector2(-1 * _speed, _rigidBody.velocity.y);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Idle");
    }
}