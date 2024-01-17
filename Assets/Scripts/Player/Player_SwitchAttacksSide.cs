using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player_SwitchAttacksSide : MonoBehaviour
{
    
    [SerializeField] AnimatorOverrideController overriderSwitchToAttack02;
    [SerializeField] AnimatorOverrideController overriderSwitchToAttack01;
    [SerializeField] CheckSide sideChecker;
    [SerializeField] Animator animator;
    

   
    public void OnReplaceAttacksSide()
    {
        if (sideChecker.dirNum < 0)
        {
            animator.runtimeAnimatorController = overriderSwitchToAttack02;
            
        }
        else
        {
            animator.runtimeAnimatorController = overriderSwitchToAttack01;
        }
        
    }
}
