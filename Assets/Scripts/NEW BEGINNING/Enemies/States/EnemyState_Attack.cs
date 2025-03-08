using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState_Attack : State
{
    //Any class inheriting from this should call at some point OnAttackFinished() to change to return to Agroo
    //Also, in the OnDisable remember to call base.OnDisable() for the cooldowb
    //Also, in the Editor, the AGROO State should be the direct parent of all ATTACK States


    //Revisa lo que sigue o no necesari aqui 
    [SerializeField] string ShortDescription;
    public Enemy_AttackRangeDetector rangeDetector;
    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public bool isActive;
    public bool notOvniInvertable = false;

    [Header("Stats:")]
    public float Damage;
    public int Probability;
    public float KnockBack;
    [HideInInspector] public float Hitstop;
    [Header("Cooldown")]
    float lastAttackFinishedTime;
    public bool isInCooldown()
    {
        if (Time.time - lastAttackFinishedTime < CooldownTime)
        {
            return true;
        }
        else { return false; }
    }
    public bool HasCooldown;
    public float CooldownTime;

    public void isInRange() { isActive = true; }
    public void isNotInRange() { isActive = false; }

    public override void OnDisable()
    {
        lastAttackFinishedTime = Time.time;
    }
    public void OnAttackFinished()
    {
        EnemyState_Agroo AgrooState = transform.parent.GetComponent<EnemyState_Agroo>(); //Una mica fragil?
        stateMachine.ChangeState(AgrooState);
    }
}
