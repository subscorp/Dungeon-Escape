using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    public void Update()
    {
        transform.Translate(Vector3.right * 3 * Time.deltaTime);
    }

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            //Player player = other.GetComponent<Player>();
            //player.Damage();
            Destroy(gameObject);
        }
    }

}
