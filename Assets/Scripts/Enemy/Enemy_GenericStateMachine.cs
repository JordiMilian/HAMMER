using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class Enemy_GenericStateMachine : MonoBehaviour
{
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;

    [SerializeField] Enemy_AgrooDetection agrooDetection;
    [SerializeField] Generic_OnTriggerEnterEvents agrooDetectionTrigger;
    [SerializeField] Enemy_EventSystem eventSystem;
    public enum States
    {
        Idle, Agroo
    }
    public States CurrentState = States.Idle;
    private void Start()
    {
        if (CurrentState == States.Idle) { OnIdleState(this, new EventArgsTriggererInfo("null", new Collider2D())); }
        if (CurrentState == States.Agroo) { OnAgrooState(this, new EventArgsTriggererInfo("null", new Collider2D())); }
    }
    private void OnEnable()
    {
        agrooDetectionTrigger.ActivatorTags.Add(TagsCollection.Instance.Player_SinglePointCollider);
        agrooDetectionTrigger.OnTriggerEntered += OnAgrooState;
        agrooDetectionTrigger.OnTriggerExited += OnIdleState;
        //agrooDetection.OnPlayerDetected += OnAgrooState;
        //agrooDetection.OnPlayerExited += OnIdleState;
    }
    private void OnDisable()
    {
        agrooDetectionTrigger.OnTriggerEntered -= OnAgrooState;
        agrooDetectionTrigger.OnTriggerExited -= OnIdleState;
        //agrooDetection.OnPlayerDetected -= OnAgrooState;
        //agrooDetection.OnPlayerExited -= OnIdleState;
    }

    void OnIdleState(object sender, EventArgsTriggererInfo args)
    {
        if (eventSystem.OnPlayerOutOfRange != null) eventSystem.OnPlayerOutOfRange(this, EventArgs.Empty);
        agrooMovement.enabled = false;
        attackProvider.enabled = false;

        idleMovement.enabled = true;
        CurrentState = States.Idle;
    }
    void OnAgrooState(object sender, EventArgsTriggererInfo args)
    {
        if (eventSystem.OnAgrooPlayer != null) eventSystem.OnAgrooPlayer(this, EventArgs.Empty);
        agrooMovement.enabled = true;
        attackProvider.enabled = true;

        idleMovement.enabled = false;
        CurrentState = States.Agroo;
    }
}
