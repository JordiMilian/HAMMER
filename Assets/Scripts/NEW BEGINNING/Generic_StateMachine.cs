using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Generic_StateMachine : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Action<State> OnStateChanged;
    [SerializeField] bool debugChangeState;
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

        
        OnStateChanged?.Invoke(currentState); 
    }
    public void ChangeState(State newState)
    {
        if(debugChangeState)
        {
            Debug.Log($"Changing state from {currentState} => {newState}");
        }
        SetNewState(newState);
    }
}
