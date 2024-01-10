using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isDoorOpen;
    public bool isClosingDoor;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Collider2D blockingCollider;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player_SinglePointCollider"))
        {
            OpenDoor();
        }
    }
       
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_SinglePointCollider"))
        {
            CloseDoor();
        }
            
    }

    public void CloseDoor()
    {
        blockingCollider.enabled = true;
        doorAnimator.SetTrigger("Close");
    }
    public void OpenDoor()
    {
        blockingCollider.enabled = false;
        doorAnimator.SetTrigger("Open");
    }

}
