using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_StateMachine : MonoBehaviour
{
    public Generic_EventSystem eventSystem;
    public enum States
    {
        Idle, Agroo, Start, Dead, Inactive, Fase01, Fase02, Transitioning,
    }
    public States CurrentState = States.Idle;
    private void OnEnable()
    {
        eventSystem.OnDeath += OnDeathState;
    }
    private void OnDisable()
    {
        eventSystem.OnDeath -= OnDeathState;
    }
    public virtual void OnDeathState(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        CurrentState = States.Dead;
        //StartCoroutine(delayDestroy());
    }
}
