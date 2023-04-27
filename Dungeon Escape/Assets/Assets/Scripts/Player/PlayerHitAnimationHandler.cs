using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use this class, in the animator of Player->Sprite, set the transition from AnyState to Hit with isTrigger.True, and find out how to transition to idle after Hit is finished

public class PlayerHitAnimationHandler : MonoBehaviour
{
    private bool isHit = false;
    [SerializeField]
    private Player _player;

    private void Start()
    {
        GameObject.Find("Player").GetComponent<Player>();
    }

    public void AnimationStart()
    {
        // Called by the animation event when it starts
        _player.AnimationStart();
    }

    public void AnimationEnd()
    {
        // Called by the animation event when it ends
        Debug.Log("AnimationEnd in PlayerHitAnimationHandler");
        _player.AnimationEnd();
    }
}
