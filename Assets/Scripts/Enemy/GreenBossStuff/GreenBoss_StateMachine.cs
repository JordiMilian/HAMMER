using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class GreenBoss_StateMachine : MonoBehaviour
{
    [SerializeField] Generic_HealthSystem bossHealthSystem;

    public enum StatesGreenBoss
    {
        Idle, Fase01, Fase02,
    }
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] GameObject Fase01_behaviour;
    [SerializeField] GameObject Fase02_behaviour;

    [SerializeField] Generic_OnTriggerEnterEvents agrooDetectionTrigger;
    
    [SerializeField] GreenBoss_EventSystem eventSystem;
    [SerializeField] Animator greenBossAnimator;
    [SerializeField] AnimatorOverrideController Fase01Animator;
    [SerializeField] AnimatorOverrideController Fase02Animator;
    
    public StatesGreenBoss CurrentState = StatesGreenBoss.Idle;
    private void Start()
    {
        if (CurrentState == StatesGreenBoss.Idle) { OnIdleState(this, new EventArgsTriggererInfo("null", new Collider2D())); }
        if (CurrentState == StatesGreenBoss.Fase01 || CurrentState == StatesGreenBoss.Fase02) { CheckHealthForState(this, EventArgs.Empty); }
    }
    private void OnEnable()
    {
        agrooDetectionTrigger.ActivatorTags.Add(TagsCollection.Instance.Player_SinglePointCollider);
        agrooDetectionTrigger.OnTriggerEntered += OnAgrooState;
        eventSystem.OnUpdatedHealth += CheckHealthForState;
        agrooDetectionTrigger.OnTriggerExited += OnIdleState;
    }
    private void OnDisable()
    {
        agrooDetectionTrigger.OnTriggerEntered -= OnAgrooState;
        eventSystem.OnUpdatedHealth -= CheckHealthForState;
        agrooDetectionTrigger.OnTriggerExited -= OnIdleState;
    }

    void OnIdleState(object sender, EventArgsTriggererInfo args)
    {
        if (eventSystem.OnPlayerOutOfRange != null) eventSystem.OnPlayerOutOfRange(this, EventArgs.Empty);
        Fase01_behaviour.SetActive(false);
        Fase02_behaviour.SetActive(false);

        idleMovement.enabled = true;
        CurrentState = StatesGreenBoss.Idle;
    }
    void OnAgrooState(object sender, EventArgsTriggererInfo args)
    {
        if (eventSystem.OnAgrooPlayer != null) eventSystem.OnAgrooPlayer(this, EventArgs.Empty);
        CheckHealthForState(this, EventArgs.Empty);
    }
    void CheckHealthForState(object sender, EventArgs args)
    {
        if(bossHealthSystem.CurrentHealth < bossHealthSystem.MaxHealth/2)
        {
            OnFase02State();
        }
        else
        {
            OnFase01State();
        }
    }
    void OnFase01State()
    {
        greenBossAnimator.runtimeAnimatorController = Fase01Animator;
        if(eventSystem.OnPhase01 != null) eventSystem.OnPhase01(this, EventArgs.Empty);
        Fase01_behaviour.SetActive(true);
        Fase02_behaviour.SetActive(false);

        idleMovement.enabled = false;
        CurrentState = StatesGreenBoss.Fase01;
    }
    void OnFase02State()
    {
        greenBossAnimator.runtimeAnimatorController = Fase02Animator;
        if (eventSystem.OnPhase02 != null) eventSystem.OnPhase02(this, EventArgs.Empty);
        Debug.Log("FASE 02");
        Fase01_behaviour.SetActive(false);
        Fase02_behaviour.SetActive(true);

        idleMovement.enabled = false;
        CurrentState = StatesGreenBoss.Fase02;
    }
}
