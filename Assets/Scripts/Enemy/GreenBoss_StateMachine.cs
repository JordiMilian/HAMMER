using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] Enemy_AgrooMovement agrooMovement;
    [SerializeField] Enemy_AttacksProviderV2 Fase01_attackProvider;
    [SerializeField] Enemy_AttacksProviderV2 Fase02_attackProvider;

    [SerializeField] Enemy_AgrooDetection agrooDetection;
    
    public StatesGreenBoss CurrentState = StatesGreenBoss.Idle;
    private void Start()
    {
        if (CurrentState == StatesGreenBoss.Idle) { OnIdleState(this, EventArgs.Empty); }
        if (CurrentState == StatesGreenBoss.Fase01 || CurrentState == StatesGreenBoss.Fase02) { CheckHealthForState(this, EventArgs.Empty); }
    }
    private void OnEnable()
    {
        agrooDetection.OnPlayerDetected += CheckHealthForState;
        bossHealthSystem.OnUpdatedHealth += CheckHealthForState;
        agrooDetection.OnPlayerExited += OnIdleState;
    }
    private void OnDisable()
    {
        agrooDetection.OnPlayerDetected -= CheckHealthForState;
        bossHealthSystem.OnUpdatedHealth -= CheckHealthForState;
        agrooDetection.OnPlayerExited -= OnIdleState;
    }

    void OnIdleState(object sender, EventArgs args)
    {
        agrooMovement.enabled = false;
        Fase01_attackProvider.enabled = false;

        idleMovement.enabled = true;
        CurrentState = StatesGreenBoss.Idle;
    }
    void CheckHealthForState(object sender, EventArgs args)
    {
        Debug.Log("checking life)");
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
        Fase01_behaviour.SetActive(true);
        Fase02_behaviour.SetActive(false);

        idleMovement.enabled = false;
        CurrentState = StatesGreenBoss.Fase01;
    }
    void OnFase02State()
    {
        Debug.Log("FASE 02");
        Fase01_behaviour.SetActive(false);
        Fase02_behaviour.SetActive(true);

        idleMovement.enabled = false;
        CurrentState = StatesGreenBoss.Fase02;
    }
}
