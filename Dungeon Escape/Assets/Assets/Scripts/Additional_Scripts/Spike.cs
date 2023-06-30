using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Player" && !GameManager.Instance.PlayerHitSpike)
        {
            GameManager.Instance.PlayerHitSpike = true;
            collider.GetComponent<Player>().HitSpike();
        }
    }
}
