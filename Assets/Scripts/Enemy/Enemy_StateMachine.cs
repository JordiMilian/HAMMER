using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class Enemy_StateMachine : Generic_StateMachine
{
    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] Generic_OnTriggerEnterEvents inRangeDetectionTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents outOfRangeDetectionTrigger;
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
        enemyRefs.enemyEvents.CallAgrooState += ActivateAgroo;
        enemyRefs.enemyEvents.CallIdleState += ActivateIdle;
        enemyRefs.enemyEvents.OnDeath += OnDeathState;
    }
    private void OnDisable()
    {
        inRangeDetectionTrigger.OnTriggerEntered -= PlayerInRange;
        outOfRangeDetectionTrigger.OnTriggerExited -= PlayerOutOfRange;
        enemyRefs.enemyEvents.CallAgrooState += ActivateAgroo;
        enemyRefs.enemyEvents.CallIdleState += ActivateIdle;
    }
    void PlayerInRange(Collider2D collision)
    {
        enemyRefs.enemyEvents.CallAgrooState?.Invoke();
    }
    void PlayerOutOfRange(Collider2D collision)
    {
        enemyRefs.enemyEvents.CallIdleState?.Invoke();
    }
    void ActivateIdle()
    {
        if (CurrentState != States.Idle)
        {
            if (enemyRefs.enemyEvents.OnIdleState != null) enemyRefs.enemyEvents.OnIdleState();
            enemyRefs.agrooMovement.enabled = false;
            enemyRefs.attackProvider.enabled = false;

            enemyRefs.idleMovement.enabled = true;
            CurrentState = States.Idle;
        } 
    }
    void ActivateAgroo()
    {
        if(CurrentState != States.Agroo)
        {
            if (enemyRefs.enemyEvents.OnAgrooState != null) enemyRefs.enemyEvents.OnAgrooState();
            enemyRefs.agrooMovement.enabled = true;
            enemyRefs.attackProvider.enabled = true;

            enemyRefs.idleMovement.enabled = false;
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
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(gameObject);
    }
}
