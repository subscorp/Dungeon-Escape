using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEvent : MonoBehaviour
{
    private Spider _spider;
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spider = GameObject.Find("Spider_Enemy").GetComponent<Spider>();
    }

    public void Fire()
    {
        _spider.Attack();
    }

    public void StartAttackSFX()
    {
        if (Vector3.Distance(_spider.transform.position, _player.transform.position) < 7f)
            AudioManager.Instance.PlaySpiderSpit();
    }
}
