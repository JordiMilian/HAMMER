using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    bool isDoorOpen;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Collider2D blockingCollider;
    [SerializeField] AudioClip OpenAudio, CloseAudio;
    public Action OnDoorOpened;
    public Action OnDoorClosed;

    [SerializeField] Transform AutoDoorOpenerCollider;
    [SerializeField] Transform AutoDoorCloserCollider;
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
            //Game_AudioPlayerSingleton.Instance.playSFXclip(CloseAudio);
            isDoorOpen = false;
            OnDoorClosed?.Invoke();
        } 
    }
    public void InstaClose()
    {
        if(isDoorOpen)
        {
            doorAnimator.SetTrigger("InstaClose");
            isDoorOpen = false;
            OnDoorClosed?.Invoke();
            EV_CloseCollider();
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
            //Game_AudioPlayerSingleton.Instance.playSFXclip(OpenAudio);
            isDoorOpen = true;
            OnDoorOpened?.Invoke();
        }
    }
    public void InstaOpen()
    {
        if (!isDoorOpen)
        {
            doorAnimator.SetTrigger("InstaOpen");
            isDoorOpen = true;
            OnDoorOpened?.Invoke();
            EV_OpenCollider();
        }
    }
    public void EV_OpenCollider()
    {
        blockingCollider.enabled = false;
    }

    public void DisableAutoDoorOpener()
    {
        Collider2D[] colliders = AutoDoorOpenerCollider.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
    public void EnableAutoDoorOpener()
    {
        Collider2D[] colliders = AutoDoorOpenerCollider.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
    public void DisableAutoDoorCloser()
    {
        Collider2D[] colliders = AutoDoorCloserCollider.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
    public void EnableAutoDoorCloser()
    {
        Collider2D[] colliders = AutoDoorCloserCollider.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
