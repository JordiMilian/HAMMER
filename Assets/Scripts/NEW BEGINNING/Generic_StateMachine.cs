using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Generic_StateMachine : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Action<State> OnStateChanged;
    public State currentState
    {
        get;
        private set;
    }
    void SetNewState(State newState)
    {
        //Debug.Log($"{gameObject.name} changed state from {currentState} to {newState}:");
        if (currentState != null)
        {
            currentState.gameObject.SetActive(false);
        }
        
        currentState = newState;

        currentState.InitializeState(this, animator);
        currentState.gameObject.SetActive(true);

        
        OnStateChanged?.Invoke(currentState); //No use for this yet, but maybe some day
    }
    public void ChangeState(State newState)
    {
        SetNewState(newState);
    }
}
