using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState
{

    [SerializeField] PlayerState PlayerState_PerformParry, PlayerState_Rolling, PlayerState_SpecialHeal, PlayerState_SpecialAttack;
    Player_References playerRefs;
    public override void OnEnable()
    {
        stateMachine.EV_ReturnInput();
        stateMachine.EV_CanTransition();

        //Change animation to Idle loop
        animator.CrossFade("Idle", 0.1f);

        //Subscribe to all posiblle movements 
        InputDetector.Instance.OnParryPressed += RequestParry;
        InputDetector.Instance.OnRollPressed += RequestRoll;
        InputDetector.Instance.OnSpecialHealPressed += RequestSpecialHeal;
        InputDetector.Instance.OnSpecialAttackPressed += RequestSpecialAttack;

        //Start recovering stamina
        playerRefs.playerStamina.StartRecovering();
    }
    public override void OnDisable()
    {
        InputDetector.Instance.OnParryPressed -= RequestParry;
        InputDetector.Instance.OnRollPressed -= RequestRoll;
        InputDetector.Instance.OnSpecialHealPressed -= RequestSpecialHeal;
        InputDetector.Instance.OnSpecialAttackPressed -= RequestSpecialAttack;

        playerRefs.playerStamina.StopRecovering();
    }
    void RequestParry() { stateMachine.RequestChangeState(PlayerState_PerformParry); }
    void RequestRoll() { stateMachine.RequestChangeState(PlayerState_Rolling); }
    void RequestSpecialHeal()
    { 
        if(playerRefs.specialAttack.isChargeFilled())
        {
            stateMachine.RequestChangeState(PlayerState_SpecialHeal);
        }
        //Feedback if not enough charge here?? Or in Player_SpecialAttack??
    }
    void RequestSpecialAttack()
    {
        if(playerRefs.specialAttack.isChargeFilled())
        {
            stateMachine.RequestChangeState(PlayerState_SpecialAttack);
        }
    }
}
