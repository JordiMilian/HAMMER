using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_Controller : MonoBehaviour, IDamageReceiver
{
    [SerializeField] Generic_StateMachine tiedStateMachine;
    [SerializeField] State aliveState, deadState;
    [SerializeField] EntityStats tiedEnemy_baseStats;
    private void Awake()
    {
        tiedStateMachine.ChangeState(aliveState); //This state should just control the sprites to show and the amount of hits to die. also the transition to dead
    }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        //play animation from animator directament o que? vfx si eso i ja, coses generiques
    }


}
