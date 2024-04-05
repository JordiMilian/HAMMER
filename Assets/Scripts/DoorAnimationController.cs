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
        isDoorOpen = false;
    }
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
    public void EV_OpenCollider()
    {
        blockingCollider.enabled = false;
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
}
