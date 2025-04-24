using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom_RegularRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            GameController.Instance.OnExitedRoom();
        }
    }
}
