using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    bool isDoorOpen;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Collider2D blockingCollider;

    private void Start()
    {
        CloseDoor();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            CloseDoor();
        }
    }
    public void CloseDoor()
    {
        if (isDoorOpen)
        {
            doorAnimator.SetTrigger("Close");
            isDoorOpen = false;
        } 
    }
    public void EV_CloseCollider()
    {
        blockingCollider.enabled = true;
    }

    public void OpenDoor()
    {
        if (!isDoorOpen)
        {
            doorAnimator.SetTrigger("Open");
            isDoorOpen = true;
        }
    }
    public void EV_OpenCollider()
    {
        blockingCollider.enabled = false;
    }
}
