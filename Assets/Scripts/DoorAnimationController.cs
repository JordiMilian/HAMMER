using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    bool isDoorOpen;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Collider2D blockingCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player_SinglePointCollider"))
        {
            CloseDoor();
        }
    }
    public void CloseDoor()
    {
        if (isDoorOpen)
        {
            blockingCollider.enabled = true;
            doorAnimator.SetTrigger("Close");
            isDoorOpen = false;
        }
        
    }
    public void OpenDoor()
    {
        if (!isDoorOpen)
        {
            blockingCollider.enabled = false;
            doorAnimator.SetTrigger("Open");
            isDoorOpen = true;
        }
    }
}
