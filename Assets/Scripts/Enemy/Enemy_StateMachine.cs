using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class Enemy_StateMachine : MonoBehaviour
{
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;

    [SerializeField] Generic_OnTriggerEnterEvents inRangeDetectionTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents outOfRangeDetectionTrigger;
    [SerializeField] Enemy_EventSystem eventSystem;
    public enum States
    {
        Idle, Agroo, Start
    }
    public States CurrentState = States.Idle;
    private void Start()
    {
        ActivateIdle(); 
    }
    private void OnEnable()
    {
        inRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        outOfRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        inRangeDetectionTrigger.OnTriggerEntered += PlayerInRange;
        outOfRangeDetectionTrigger.OnTriggerExited += PlayerOutOfRange;
        eventSystem.CallAgrooState += ActivateAgroo;
        eventSystem.CallIdleState += ActivateIdle;
    }
    private void OnDisable()
    {
        inRangeDetectionTrigger.OnTriggerEntered -= PlayerInRange;
        outOfRangeDetectionTrigger.OnTriggerExited -= PlayerOutOfRange;
        eventSystem.CallAgrooState += ActivateAgroo;
        eventSystem.CallIdleState += ActivateIdle;
    }
    void PlayerInRange(object sender, EventArgsCollisionInfo args)
    {
        eventSystem.CallAgrooState?.Invoke();
    }
    void PlayerOutOfRange(object sender, EventArgsCollisionInfo args)
    {
        eventSystem.CallIdleState?.Invoke();
    }
    void ActivateIdle()
    {
        if (CurrentState != States.Idle)
        {
            if (eventSystem.OnIdleState != null) eventSystem.OnIdleState();
            agrooMovement.enabled = false;
            attackProvider.enabled = false;

            idleMovement.enabled = true;
            CurrentState = States.Idle;
        } 
    }
    void ActivateAgroo()
    {
        if(CurrentState != States.Agroo)
        {
            if (eventSystem.OnAgrooState != null) eventSystem.OnAgrooState();
            agrooMovement.enabled = true;
            attackProvider.enabled = true;

            idleMovement.enabled = false;
            CurrentState = States.Agroo;
        }
    }
}
