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
        OnIdleState(this, new EventArgsCollisionInfo( new Collider2D())); 
    }
    private void OnEnable()
    {
        inRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        outOfRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        inRangeDetectionTrigger.OnTriggerEntered += OnAgrooState;
        outOfRangeDetectionTrigger.OnTriggerExited += OnIdleState;
        //agrooDetection.OnPlayerDetected += OnAgrooState;
        //agrooDetection.OnPlayerExited += OnIdleState;
    }
    private void OnDisable()
    {
        inRangeDetectionTrigger.OnTriggerEntered -= OnAgrooState;
        outOfRangeDetectionTrigger.OnTriggerExited -= OnIdleState;
        //agrooDetection.OnPlayerDetected -= OnAgrooState;
        //agrooDetection.OnPlayerExited -= OnIdleState;
    }

    void OnIdleState(object sender, EventArgsCollisionInfo args)
    {
        if (CurrentState != States.Idle)
        {
            if (eventSystem.OnPlayerOutOfRange != null) eventSystem.OnPlayerOutOfRange();
            agrooMovement.enabled = false;
            attackProvider.enabled = false;

            idleMovement.enabled = true;
            CurrentState = States.Idle;
        } 
    }
    void OnAgrooState(object sender, EventArgsCollisionInfo args)
    {
        if(CurrentState != States.Agroo)
        {
            if (eventSystem.OnAgrooPlayer != null) eventSystem.OnAgrooPlayer();
            agrooMovement.enabled = true;
            attackProvider.enabled = true;

            idleMovement.enabled = false;
            CurrentState = States.Agroo;
        }
    }
}
