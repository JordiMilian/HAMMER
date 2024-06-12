using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorOnNear : MonoBehaviour
{
    [SerializeField] DoorAnimationController doorAnimationController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            doorAnimationController.OpenDoor();
        }
    }
}
