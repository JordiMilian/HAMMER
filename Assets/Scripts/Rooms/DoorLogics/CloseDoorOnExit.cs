using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorOnExit : MonoBehaviour
{
   [SerializeField] DoorAnimationController doorAnimationController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            doorAnimationController.CloseDoor();
        }
    }
}
