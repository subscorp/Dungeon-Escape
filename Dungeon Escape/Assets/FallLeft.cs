using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeft : StateMachineBehaviour
{
    private float _speed = 6f;
    private float _jumpForce = 10f;
    private float _prevVelocityY;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    private Boss _boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (IsGrounded())
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
            _rigidBody.velocity = new Vector2(-1 * _speed, _rigidBody.velocity.y);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private bool IsGrounded()
    {
        Vector3 pos = _boss.transform.position;
        //pos.y -= 10;
        Debug.DrawRay(pos, Vector2.down * 0.25f, Color.red);
        RaycastHit2D hitInfo = Physics2D.Raycast(pos, Vector2.down, 0.25f, 1 << 8);
        if (hitInfo.collider != null)
            return true;

        return false;
    }
}