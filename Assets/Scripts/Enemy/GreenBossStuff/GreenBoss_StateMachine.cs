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
        Idle, Fase01, Fase02, Transitioning,
    }
    [SerializeField] Enemy_IdleMovement idleMovement;
    [SerializeField] Enemy_AgrooMovement agrooMovement;
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
        agrooMovement.enabled = false;

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
        switch(CurrentState)
        {
            case StatesGreenBoss.Idle:
                OnFase01State();
                break;
            case StatesGreenBoss.Fase01:
                if (bossHealthSystem.CurrentHealth < bossHealthSystem.MaxHealth / 2)
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
    void OnFase01State()
    {
        //replace animations in the animator
        greenBossAnimator.runtimeAnimatorController = Fase01Animator;
        //call the event
        if(eventSystem.OnPhase01 != null) eventSystem.OnPhase01(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_behaviour.SetActive(true);
        Fase02_behaviour.SetActive(false);
        idleMovement.enabled = false;
        agrooMovement.enabled = true;
        //Set the Enum
        CurrentState = StatesGreenBoss.Fase01;
    }
    public void OnFase02State()
    {
        //replace animations in the animator
        greenBossAnimator.runtimeAnimatorController = Fase02Animator;
        //Deactivate animator bool
        greenBossAnimator.SetBool("isTransitioning", false);
        //Call the event
        if (eventSystem.OnPhase02 != null) eventSystem.OnPhase02(this, EventArgs.Empty);
        //Deactivate the scripts
        Fase01_behaviour.SetActive(false);
        Fase02_behaviour.SetActive(true);
        idleMovement.enabled = false;
        agrooMovement.enabled = true;

        Fase02_behaviour.GetComponent<Enemy_AttacksProviderV2>().ResetAllTriggers();
        //Set the enum
        CurrentState = StatesGreenBoss.Fase02;
        Debug.Log("PHASE02");
    }
    void OnTransitioning()
    {
        //Call the event
        if(eventSystem.OnTransitionBegin != null) eventSystem.OnTransitionBegin(this,EventArgs.Empty);
        //Deactivate the providers
        Fase01_behaviour.SetActive(false);
        Fase02_behaviour.SetActive(false);
        //Activate ANimator bool
        greenBossAnimator.SetBool("isTransitioning", true);
        //Set Enum
        CurrentState = StatesGreenBoss.Transitioning;
    }
}
