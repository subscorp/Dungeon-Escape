using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveArrow : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player")
            return;

        _renderer.enabled = true;   
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name != "Player")
            return;

        _renderer.enabled = false;
    }
}
