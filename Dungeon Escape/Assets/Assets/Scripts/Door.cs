using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != "Player")
            return;

        if (gameObject.name == "Door_A")
        {
            GameManager.Instance.PlayerInFrontOfDoorA = true;
            GameManager.Instance.PlayerInFrontOfDoorB = false;
        }
        else
        {
            GameManager.Instance.PlayerInFrontOfDoorB = true;
            GameManager.Instance.PlayerInFrontOfDoorA = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name != "Player")
            return;

        if (gameObject.name == "Door_A")
            GameManager.Instance.PlayerInFrontOfDoorA = false;
        else if(gameObject.name == "Door_B")
            GameManager.Instance.PlayerInFrontOfDoorB = false;
    }
}
