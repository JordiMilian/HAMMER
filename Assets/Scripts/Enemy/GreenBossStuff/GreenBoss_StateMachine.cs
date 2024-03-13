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
        Idle, Fase01, Fase02, Transitioning, Start,
    }
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 Fase01_provider;
    [SerializeField] Enemy_AttacksProviderV2 Fase02_provider;

    [SerializeField] Generic_OnTriggerEnterEvents inRangeDetectionTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents outOfRangeDetectionTrigger;
    
    [SerializeField] GreenBoss_EventSystem eventSystem;
    [SerializeField] Animator greenBossAnimator;
    [SerializeField] AnimatorOverrideController Fase01Animator;
    [SerializeField] AnimatorOverrideController Fase02Animator;
    
    public StatesGreenBoss CurrentState = StatesGreenBoss.Idle;
    private void Start()
    {
        if(CurrentState == StatesGreenBoss.Fase02) { replaceAnimatorOverride(Fase02Animator); }
         OnIdleState(this, new EventArgsCollisionInfo( new Collider2D())); 
    }
    private void OnEnable()
    {
        inRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        outOfRangeDetectionTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        inRangeDetectionTrigger.OnTriggerEntered += OnAgrooState;
        eventSystem.OnUpdatedHealth += CheckHealthForState;
        outOfRangeDetectionTrigger.OnTriggerExited += OnIdleState;
    }
    private void OnDisable()
    {

        inRangeDetectionTrigger.OnTriggerEntered -= OnAgrooState;
        eventSystem.OnUpdatedHealth -= CheckHealthForState;
        outOfRangeDetectionTrigger.OnTriggerExited -= OnIdleState;
    }

    
    void CheckHealthForState()
    {
        switch(CurrentState)
        {
            case StatesGreenBoss.Idle:
                if (bossHealthSystem.CurrentHP.Value < bossHealthSystem.MaxHP.Value / 2)
                {
                    OnFase02State();
                }
                else { OnFase01State(); }
                break;
            case StatesGreenBoss.Fase01:
                if (bossHealthSystem.CurrentHP.Value < bossHealthSystem.MaxHP.Value / 2)
                {
                    OnTransitioning();
                }
                break;
            case StatesGreenBoss.Fase02:
                break;
            case StatesGreenBoss.Transitioning:
                break;
        }
    }
    void OnIdleState(object sender, EventArgsCollisionInfo args)
    {
        if(CurrentState != StatesGreenBoss.Idle)
        {
            if (eventSystem.OnIdleState != null) eventSystem.OnIdleState();
            Fase01_provider.isProviding = false;
            Fase02_provider.isProviding = false;
            agrooMovement.enabled = false;

            idleMovement.enabled = true;
            CurrentState = StatesGreenBoss.Idle;
        }
        
    }
    void OnAgrooState(object sender, EventArgsCollisionInfo args)
    {
        if(CurrentState != StatesGreenBoss.Fase01 && CurrentState != StatesGreenBoss.Fase02 && CurrentState != StatesGreenBoss.Transitioning)
        {
            if (eventSystem.OnAgrooState != null) eventSystem.OnAgrooState();
            CheckHealthForState();
        }
    }
    void OnFase01State()
    {
        //replace animations in the animator
        replaceAnimatorOverride(Fase01Animator);
        //call the event
        if(eventSystem.OnPhase01 != null) eventSystem.OnPhase01(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_provider.isProviding = true;
        Fase02_provider.isProviding = false;
        idleMovement.enabled = false;
        agrooMovement.enabled = true;
        //Set the Enum
        CurrentState = StatesGreenBoss.Fase01;
    }
    public void OnFase02State()
    {
        //replace animations in the animator
        replaceAnimatorOverride(Fase02Animator);
        //Deactivate animator bool
        greenBossAnimator.SetBool("isTransitioning", false);
        //Call the event
        if (eventSystem.OnPhase02 != null) eventSystem.OnPhase02(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_provider.isProviding = false;
        Fase02_provider.isProviding = true;
        idleMovement.enabled = false;
        agrooMovement.enabled = true;

        //Set the enum
        CurrentState = StatesGreenBoss.Fase02;
        Debug.Log("PHASE02");
    }
    void replaceAnimatorOverride(AnimatorOverrideController overrideController)
    {
        greenBossAnimator.runtimeAnimatorController = overrideController;
    }
    void OnTransitioning()
    {
        //Call the event
        if(eventSystem.OnTransitionBegin != null) eventSystem.OnTransitionBegin(this,EventArgs.Empty);
        //Deactivate the providers
        Fase01_provider.isProviding = false;
        Fase02_provider.isProviding = false;
        //Activate ANimator bool
        greenBossAnimator.SetBool("isTransitioning", true);
        //Set Enum
        CurrentState = StatesGreenBoss.Transitioning;
    }
}
