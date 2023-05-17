using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    private Player _player;
    private Spider _spider1, _spider2, _spider3;
    private float _speed = 3.0f;
    private float _caveGroundY = -23.2f;
    private Vector3 _direction;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spider1 = GameObject.Find("Spider_Enemy_1").GetComponent<Spider>();
        _spider2 = GameObject.Find("Spider_Enemy_2").GetComponent<Spider>();
        _spider3 = GameObject.Find("Spider_Enemy_3").GetComponent<Spider>();
        if (Vector3.Distance(_spider1.transform.position, transform.position) < 1)
            _direction = Vector3.right;
        else if (Vector3.Distance(_spider2.transform.position, transform.position) < 1)
            _direction = new Vector3(1, -1, 0);
        else if (Vector3.Distance(_spider3.transform.position, transform.position) < 1)
            _direction = new Vector3(-1, -1, 0);

        if(_direction == Vector3.right)
            Destroy(gameObject, 5.0f);
    }

    public void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
        if(!(_direction == Vector3.right) && transform.position.y <= _caveGroundY)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Destroy(gameObject);
        }
    }

}
