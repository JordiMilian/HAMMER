using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController_ToRoll : MonoBehaviour
{
    [SerializeField] GameObject DamageCollidersRoot;
    [SerializeField] RoomCollider blockingCollider;
    private void OnTriggerEnter2D(Collider2D collision) //AutoDoorCloser
    {
        if (collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            CloseDoor();
        }
    }
    void CloseDoor()
    {
        GetComponent<Animator>().SetTrigger("Close");
        blockingCollider.collisionLayer = CollisionLayers.AllCollision;
        HideDamageCollider();
    }
    void HideDamageCollider()
    {
        DamageCollidersRoot.GetComponent<Collider2D>().enabled = false;
    }
}
