using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDiamond : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
    }
}
