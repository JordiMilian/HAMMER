using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class Enemy_StateMachine : Generic_StateMachine
{
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;

    [SerializeField] Generic_OnTriggerEnterEvents inRangeDetectionTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents outOfRangeDetectionTrigger;
    [SerializeField] Enemy_EventSystem enemyEventSystem;
    private void Start()
    {
        ActivateIdle(); 
    }
    private void OnEnable()
    {
        inRangeDetectionTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        outOfRangeDetectionTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        inRangeDetectionTrigger.OnTriggerEntered += PlayerInRange;
        outOfRangeDetectionTrigger.OnTriggerExited += PlayerOutOfRange;
        enemyEventSystem.CallAgrooState += ActivateAgroo;
        enemyEventSystem.CallIdleState += ActivateIdle;
        enemyEventSystem.OnDeath += OnDeathState;
    }
    private void OnDisable()
    {
        inRangeDetectionTrigger.OnTriggerEntered -= PlayerInRange;
        outOfRangeDetectionTrigger.OnTriggerExited -= PlayerOutOfRange;
        enemyEventSystem.CallAgrooState += ActivateAgroo;
        enemyEventSystem.CallIdleState += ActivateIdle;
    }
    void PlayerInRange(object sender, EventArgsCollisionInfo args)
    {
        enemyEventSystem.CallAgrooState?.Invoke();
    }
    void PlayerOutOfRange(object sender, EventArgsCollisionInfo args)
    {
        enemyEventSystem.CallIdleState?.Invoke();
    }
    void ActivateIdle()
    {
        if (CurrentState != States.Idle)
        {
            if (enemyEventSystem.OnIdleState != null) enemyEventSystem.OnIdleState();
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
            if (enemyEventSystem.OnAgrooState != null) enemyEventSystem.OnAgrooState();
            agrooMovement.enabled = true;
            attackProvider.enabled = true;

            idleMovement.enabled = false;
            CurrentState = States.Agroo;
        }
    }
    public override void OnDeathState(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        base.OnDeathState(sender, args);
        StartCoroutine(delayDestroy());
    }
    IEnumerator delayDestroy()
    {
        yield return new WaitForSecondsRealtime(0.08f);
        Destroy(gameObject);
    }
}
