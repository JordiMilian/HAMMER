using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_StartAttack : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player_SwitchAttacksSide switchSides = GameObject.Find("MainCharacter").GetComponent<Player_SwitchAttacksSide>();
        
        switchSides.OnReplaceAttacksSide();
    }
}
