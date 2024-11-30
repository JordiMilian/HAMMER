using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBoss_StateMachine : Generic_StateMachine
{
    //NOT USED PLS DESTROY

    /*
    [SerializeField] Generic_HealthSystem bossHealthSystem;

    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 Fase01_provider;
    [SerializeField] Enemy_AttacksProviderV2 Fase02_provider;

    [SerializeField] Generic_OnTriggerEnterEvents inRangeDetectionTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents outOfRangeDetectionTrigger;
    
    [SerializeField] GreenBoss_EventSystem GreenEventSystem;
    [SerializeField] Animator greenBossAnimator;
    [SerializeField] AnimatorOverrideController Fase01Animator;
    [SerializeField] AnimatorOverrideController Fase02Animator;
    
    private void Start()
    {
        if(CurrentState == States.Fase02) { replaceAnimatorOverride(Fase02Animator); }
         OnIdleState(new Collider2D()); 
    }
    private void OnEnable()
    {
        inRangeDetectionTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        outOfRangeDetectionTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        inRangeDetectionTrigger.OnTriggerEntered += OnAgrooState;
        //GreenEventSystem.OnUpdatedHealth += CheckHealthForState;
        outOfRangeDetectionTrigger.OnTriggerExited += OnIdleState;
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
    private void OnDisable()
    {
        inRangeDetectionTrigger.OnTriggerEntered -= OnAgrooState;
        //GreenEventSystem.OnUpdatedHealth -= CheckHealthForState;
        outOfRangeDetectionTrigger.OnTriggerExited -= OnIdleState;
    }

    
    void CheckHealthForState()
    {
        switch(CurrentState)
        {
            case States.Idle:
                if (bossHealthSystem.CurrentHP.GetValue() < bossHealthSystem.MaxHP.GetValue() / 2)
                {
                    OnFase02State();
                }
                else { OnFase01State(); }
                break;
            case States.Fase01:
                if (bossHealthSystem.CurrentHP.GetValue() < bossHealthSystem.MaxHP.GetValue() / 2)
                {
                    OnTransitioning();
                }
                break;
            case States.Fase02:
                break;
            case States.Transitioning:
                break;
        }
    }
    void OnIdleState(Collider2D collision)
    {
        if(CurrentState != States.Idle)
        {
            if (GreenEventSystem.OnIdleState != null) GreenEventSystem.OnIdleState();
            Fase01_provider.isProviding = false;
            Fase02_provider.isProviding = false;
            agrooMovement.enabled = false;

            idleMovement.enabled = true;
            CurrentState = States.Idle;
        }
        
    }
    void OnAgrooState(Collider2D collision)
    {
        if(CurrentState != States.Fase01 && CurrentState != States.Fase02 && CurrentState != States.Transitioning)
        {
            if (GreenEventSystem.OnAgrooState != null) GreenEventSystem.OnAgrooState();
            CheckHealthForState();
        }
    }
    void OnFase01State()
    {
        //replace animations in the animator
        replaceAnimatorOverride(Fase01Animator);
        //call the event
        if(GreenEventSystem.OnPhase01 != null) GreenEventSystem.OnPhase01(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_provider.isProviding = true;
        Fase02_provider.isProviding = false;
        idleMovement.enabled = false;
        agrooMovement.enabled = true;
        //Set the Enum
        CurrentState = States.Fase01;
    }
    public void OnFase02State()
    {
        //replace animations in the animator
        replaceAnimatorOverride(Fase02Animator);
        //Deactivate animator bool
        greenBossAnimator.SetBool("isTransitioning", false);
        //Call the event
        if (GreenEventSystem.OnPhase02 != null) GreenEventSystem.OnPhase02(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_provider.isProviding = false;
        Fase02_provider.isProviding = true;
        idleMovement.enabled = false;
        agrooMovement.enabled = true;

        //Set the enum
        CurrentState = States.Fase02;
        Debug.Log("PHASE02");
    }
    void replaceAnimatorOverride(AnimatorOverrideController overrideController)
    {
        greenBossAnimator.runtimeAnimatorController = overrideController;
    }
    void OnTransitioning()
    {
        //Call the event
        if(GreenEventSystem.OnTransitionBegin != null) GreenEventSystem.OnTransitionBegin(this,EventArgs.Empty);
        //Deactivate the providers
        Fase01_provider.isProviding = false;
        Fase02_provider.isProviding = false;
        //Activate ANimator bool
        greenBossAnimator.SetBool("isTransitioning", true);
        //Set Enum
        CurrentState = States.Transitioning;
    }
    */
}
