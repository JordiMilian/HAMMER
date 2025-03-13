using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_Controller : MonoBehaviour, IDamageReceiver, IKilleable
{
    [SerializeField] Generic_StateMachine tiedStateMachine;
    public State AliveState, DeadState;
    [SerializeField] EntityStats tiedEnemy_baseStats;

    

    private void Awake()
    {
        tiedStateMachine.ChangeState(AliveState); //This state should just control the sprites to show and the amount of hits to die. also the transition to dead
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_Event { get; set; }
    

    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        //play animation from animator directament o que? vfx si eso i ja, coses generiques
        OnDamageReceived_Event?.Invoke(info);
    }
    public Action<DeadCharacterInfo> OnKilled_event { get ; set; }
    public void OnKilled(DeadCharacterInfo info)
    {
        OnKilled_event?.Invoke(info);
    }
}
