using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRight : StateMachineBehaviour
{
    private float _speed = 6f;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    private Boss _boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
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
            _rigidBody.velocity = new Vector2(1 * _speed, _rigidBody.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        Vector3 pos = _boss.transform.position;
        Debug.DrawRay(pos, Vector2.down * 0.25f, Color.red);
        RaycastHit2D hitInfo = Physics2D.Raycast(pos, Vector2.down, 0.25f, 1 << 8);
        if (hitInfo.collider != null)
            return true;

        return false;
    }
}