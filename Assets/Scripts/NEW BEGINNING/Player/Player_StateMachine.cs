 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player_StateMachine : MonoBehaviour
{
    // ALL player state animations must implement (or must transition to a state animation that implements) the Animation events EV_ReturnInput and EV_CanTransition

    [SerializeField] Player_References playerRefs;
    [SerializeField] Animator animator;
    public Action<PlayerState> OnStateExited;
    public Action<PlayerState> OnStateEntered;
    PlayerState NextRequestedState;
    [SerializeField] Generic_StaminaBarController staminaBarController;
    public bool CanTransition { get; private set; }
    bool isReadingInput;

    public PlayerState currentState
    {
        get;
        private set;
    }
    void SetNewState(PlayerState newState)
    {
        #region MANAGE STAMINA
        if (newState.doesRequireStamina)
        {
            if(playerRefs.playerStamina.isEmpty)
            {
                staminaBarController.NotEnoughStaminaFeedback();
                return;
            }
        }
        playerRefs.playerStamina.RemoveStamina(newState.StaminaCost);
        #endregion

        if (currentState != null)
        {
            currentState.gameObject.SetActive(false);
            OnStateExited?.Invoke(currentState);
        }
        currentState = newState;

        CanTransition = false;
        isReadingInput = false;
        NextRequestedState = null;

        currentState.InitializeState(this, animator);
        currentState.gameObject.SetActive(true);
        Debug.Log($"Changed player state to: {currentState.name}");

        OnStateEntered?.Invoke(currentState); 
    }
    public void ForceChangeState(PlayerState forcedState)
    {
        SetNewState(forcedState);
    }
    public void RequestChangeState(PlayerState requestedState)
    {
        if (!isReadingInput) { Debug.Log("Currently not reading input for: " + requestedState.name); return; }

        if (CanTransition) { SetNewState(requestedState);}
        else
        {
            NextRequestedState = requestedState;
        }
    }
    private void Update()
    {
        if(CanTransition && NextRequestedState != null)
        {
            SetNewState(NextRequestedState);
        }
    }
    public void EV_ReturnInput()
    {
        isReadingInput = true;
    }
    public void EV_CanTransition()
    {
        CanTransition = true;
    }
}
