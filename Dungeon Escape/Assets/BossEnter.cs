using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnter : StateMachineBehaviour
{
    [SerializeField]
    private Boss _boss;
    private Player _player;
    [SerializeField]
    private SpriteRenderer _shield;

    private void Awake()
    {
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _shield = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
        _boss.ShieldOn = true;
        _shield.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (IsGrounded())
        {
            animator.SetBool("Phase1", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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