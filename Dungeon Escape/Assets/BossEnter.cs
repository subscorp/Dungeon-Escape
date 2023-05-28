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
    private Animator _gateAnim;
    private float timer = 0f;
    private float loopDuration = 0.8f;
    [SerializeField]
    private Vector3 _pointD;
    private Rigidbody2D _rigidBody;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        animator.ResetTrigger("Player_Escapes");
        _player = GameObject.Find("Player").GetComponent<Player>();
        _shield = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
        _gateAnim = GameObject.Find("Gate").GetComponent<Animator>();
        _boss.ShieldOn = true;
        _shield.enabled = true;
        _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            _boss.gameObject.AddComponent<Rigidbody2D>();
            _rigidBody = animator.GetComponentInParent<Rigidbody2D>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (IsGrounded())
        {
            animator.SetBool("Phase1", true);
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