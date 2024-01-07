using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_ParryPerformer : MonoBehaviour
{
    Animator playerAnimator;
 
    Player_AnimationEvents animationEvents;

    public event EventHandler<EventArgs_ParryInfo> OnSuccessfulParry;
    public class EventArgs_ParryInfo : EventArgs
    {
        public Vector3 vector3data;
        public EventArgs_ParryInfo(Vector3 data)
        {
            vector3data = data;
        }
    }
    private void Awake()
    {
       
        animationEvents = GetComponent<Player_AnimationEvents>();
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Parry");
        }
    }
    public void OnSuccessfulParryDetected(Vector3 closestPoint)
    {
        if (OnSuccessfulParry != null) OnSuccessfulParry(this, new EventArgs_ParryInfo(closestPoint)); 
        animationEvents.EV_HideParryColldier();
    }
   
}
