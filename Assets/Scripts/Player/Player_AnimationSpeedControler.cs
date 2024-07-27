using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationSpeedControler : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    public float attackingSpeed = 1;
    public float BaseSpeed = 1;
    bool overlappingAttack;
    bool isAttacking;
    private void OnEnable()
    {
        playerRefs.events.OnAttackStarted += AttackPerformed;
        playerRefs.events.OnAttackFinished += AttackEnded;
    }
    void AttackPerformed()
    {
        if (isAttacking) { overlappingAttack = true; }
        else
        {
            isAttacking = true;
            setSpeed();
        }
    }
    void AttackEnded()
    {
        if (overlappingAttack) { overlappingAttack = false; return; }
        else if(isAttacking)
        {
            isAttacking = false;
            unsetSpeed();
        }
    }
    void setSpeed()
    {
        playerRefs.animator.speed = attackingSpeed;
    }
    void unsetSpeed()
    {
        playerRefs.animator.speed = 1;
    }

}
