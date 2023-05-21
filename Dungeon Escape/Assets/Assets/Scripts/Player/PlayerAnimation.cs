using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Animator _swordAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _swordAnimation = transform.GetChild(1).GetComponent<Animator>();
    }

    public void Move(float move)
    {
        _animator.SetFloat("Move", Mathf.Abs(move));
    }

    public void Jump(bool jump)
    {
        _animator.SetBool("Jumping", jump);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
        if(!_animator.GetBool("Fire Attack"))
            _swordAnimation.SetTrigger("SwordAnimation");
    }

    public void SetFireAttack()
    {
        _animator.SetBool("Fire Attack", true);
    }

    public void JumpAttack()
    {
        _animator.SetTrigger("Jump Attack");
        if (!_animator.GetBool("Fire Attack"))
            _swordAnimation.SetTrigger("SwordAnimation");
    }

    public void Death()
    {
        _animator.SetTrigger("Death");
    }

    public void Hit()
    {
        _animator.SetTrigger("Hit");
    }

    public void SetGrounded(bool val)
    {
        _animator.SetBool("Grounded", val);
    }

    public float GetCurrentAnimationLength()
    {
        // Get the current Animator state
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // Get the current clip associated with the state
        AnimationClip clip = stateInfo.shortNameHash != 0 ? _animator.GetCurrentAnimatorClipInfo(0)[0].clip : null;
        Debug.Log("clip.name: " + clip.name);

        // Return the length of the current clip
        return clip != null ? clip.length : 0f;
    }
}
