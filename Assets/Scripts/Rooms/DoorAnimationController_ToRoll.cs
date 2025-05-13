using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController_ToRoll : MonoBehaviour, IDamageDealer
{
    [SerializeField] GameObject DamageCollidersRoot;
    [SerializeField] RoomCollider blockingCollider;

    public Action<DealtDamageInfo> OnDamageDealt_event { get ; set ; }

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

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
}
