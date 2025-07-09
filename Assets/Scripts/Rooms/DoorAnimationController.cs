using Pathfinding.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour, ICutsceneable
{
    bool isDoorOpen;
    [SerializeField] Animator doorAnimator;
    [SerializeField] RoomCollider blockingCollider;
    public Action OnDoorOpen;
    public Action OnDoorClose;
    public Action OnDoorInstaOpen, OnDoorInstaClose;

    [SerializeField] Transform AutoDoorOpenerCollider;
    [SerializeField] Transform AutoDoorCloserCollider;
    [SerializeField] AnimationClip openDoorAnimation;
    /*
    // HOW TO MAKE A NEW DOOR

        - Copy the Base door
        - Make the 4 required animations with a new Animator (the animator should be createad automatically): Open, Close, InstaOpen, InstaClose
        - Add the events EV_OpenCollider and EV_CloseCollider to the animations
        - Recreate the Door_01 Animator with the same exact parameter names and transitions

     */
    private void OnTriggerEnter2D(Collider2D collision) //AutoDoorCloser
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
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
            OnDoorClose?.Invoke();
        } 
    }
    public void InstaClose()
    {
        if(isDoorOpen)
        {
            doorAnimator.SetTrigger("InstaClose");
            isDoorOpen = false;
            OnDoorInstaClose?.Invoke();
            EV_CloseCollider();
        }
    }
    public void OpenDoor()
    {
        if (!isDoorOpen)
        {
            doorAnimator.SetTrigger("Open");
            //Game_AudioPlayerSingleton.Instance.playSFXclip(OpenAudio);
            isDoorOpen = true;
            OnDoorOpen?.Invoke();
        }
    }
    public void InstaOpen()
    {
        if (!isDoorOpen)
        {
            doorAnimator.SetTrigger("InstaOpen");
            isDoorOpen = true;
            OnDoorInstaOpen?.Invoke();
            EV_OpenCollider();
        }
    }
    public void EV_CloseCollider()
    {
        blockingCollider.enabled = true;
    }
    public void EV_OpenCollider()
    {
        blockingCollider.enabled = false;
    }
    #region automatic Opener and Closer
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
    #endregion
    #region OPEN WITH CUTSCENE
    public void OpenWithCutscene()
    {
        CutscenesManager.Instance.AddCutsceneable(this);
    }
    public IEnumerator ThisCutscene()
    {
        Transform doorTransform = transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        TargetGroupSingleton.Instance.RemovePlayersTarget();
        yield return new WaitForSeconds(0.2f); //Wait 
        OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera
        TargetGroupSingleton.Instance.ReturnPlayersTarget();

        playerStateMachine.ForceChangeState(playerRefs.IdleState);
    }
    public void ForceEndCutscene()
    {
        Transform doorTransform = transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        OpenDoor();
        TargetGroupSingleton.Instance.SetOnlyPlayerAndMouseTarget();
        playerStateMachine.ForceChangeState(playerRefs.IdleState);
    }
    #endregion

}
