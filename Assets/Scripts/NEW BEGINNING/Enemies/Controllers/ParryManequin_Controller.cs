using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryManequin_Controller : MonoBehaviour, IParryReceiver, IDamageReceiver
{
    [SerializeField] Generic_StateMachine manequinStateMachine;
    [SerializeField] State DamagedState, ParriedState, AttackingState;
    private void Awake()
    {
        manequinStateMachine.ChangeState(AttackingState);
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        //Trigger damaged animation
        //Flasher
        OnDamageReceived_event?.Invoke(info);
    }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public void OnParryReceived(GettingParriedInfo info)
    {
        //Should the manequin tell the door or should the door listen to the manequin?
        // The manequin should tell the door, this manquin only exists in this room, but that door is reused in other rooms
        OnParryReceived_event?.Invoke(info);
    }
}
