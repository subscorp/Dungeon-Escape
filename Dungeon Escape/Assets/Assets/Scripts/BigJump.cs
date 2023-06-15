using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigJump : MonoBehaviour
{
    const int TRIGGER_X_POSITION = 103;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            if(collider.transform.position.x < TRIGGER_X_POSITION)
                if (!GameManager.Instance.HasBootsOfFlight && !GameManager.Instance.GotKonamiCode)
            {
                UIManager.Instance.EnableOrDisableBigJumpPanel(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.EnableOrDisableBigJumpPanel(false);
    }
}
