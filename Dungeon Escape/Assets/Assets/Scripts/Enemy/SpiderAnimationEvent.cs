using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private Spider _spider1, _spider2, _spider3;
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Fire()
    {
        if (_spider1 != null)
            _spider1.Attack();
        else if(_spider2 != null)
            _spider2.Attack();
        else if(_spider3 != null)
            _spider3.Attack();
    }

    public void StartAttackSFX()
    {
        if (_spider1 != null)
        {
            if (Vector3.Distance(_spider1.transform.position, _player.transform.position) < 7f)
                AudioManager.Instance.PlaySpiderSpit();
        }
        if (_spider2 != null)
        {
            if (Vector3.Distance(_spider2.transform.position, _player.transform.position) < 7f)
                AudioManager.Instance.PlaySpiderSpit();
        }
        if (_spider3 != null)
        {
            if (Vector3.Distance(_spider3.transform.position, _player.transform.position) < 7f)
                AudioManager.Instance.PlaySpiderSpit();
        }
    }
}
