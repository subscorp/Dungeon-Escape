using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscapes : StateMachineBehaviour
{
    [SerializeField]
    private Boss _boss;
    [SerializeField]
    private Vector3 _pointC;
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private SpriteRenderer _TeleportEffectPrefab;

    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        animator.ResetTrigger("Player_Returns"); 
        _rigidBody = _boss.GetComponent<Rigidbody2D>();
        _pointC = new Vector3(126.36f, -0.39f, 0);
        AudioManager.Instance.PlayKingLaughSound();
        SpriteRenderer teleportEffect = Instantiate(_TeleportEffectPrefab, _boss.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayTeleportSFX();
        _boss.transform.position = _pointC;
        _boss.GetComponentInChildren<SpriteRenderer>().flipX = true;
        Destroy(_rigidBody);
        Destroy(teleportEffect.gameObject, 0.417f);
        AudioManager.Instance.FadeOutBossMusic();
        _boss.Health = 9;
        animator.SetBool("Phase1", false);
        animator.SetBool("Phase2", false);
        animator.SetBool("Phase3", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Player_Escapes");

        //AudioManager.Instance.PlayBossMusic();
    }
}
