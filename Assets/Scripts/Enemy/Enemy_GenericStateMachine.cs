using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_GenericStateMachine : MonoBehaviour
{
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;

    [SerializeField] Enemy_AgrooDetection agrooDetection;
     public enum States
    {
        Idle, Agroo
    }
    public States CurrentState = States.Idle;
    private void Start()
    {
        if (CurrentState == States.Idle) { OnIdleState(this, EventArgs.Empty); }
        if (CurrentState == States.Agroo) { OnAgrooState(this, EventArgs.Empty); }
    }
    private void OnEnable()
    {
        agrooDetection.OnPlayerDetected += OnAgrooState;
        agrooDetection.OnPlayerExited += OnIdleState;
    }
    private void OnDisable()
    {
        agrooDetection.OnPlayerDetected -= OnAgrooState;
        agrooDetection.OnPlayerExited -= OnIdleState;
    }

    void OnIdleState(object sender, EventArgs args)
    {
        agrooMovement.enabled = false;
        attackProvider.enabled = false;

        idleMovement.enabled = true;
        CurrentState = States.Idle;
    }
    void OnAgrooState(object sender, EventArgs args)
    {
        agrooMovement.enabled = true;
        attackProvider.enabled = true;

        idleMovement.enabled = false;
        CurrentState = States.Agroo;
    }
}
