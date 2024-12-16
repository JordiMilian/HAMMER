using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static Generic_EventSystem;

public class Enemy01 : MonoBehaviour
{
    [SerializeField] Enemy_References enemyRefs;

    [SerializeField] FloatVariable SlowMoPercentage;
    AnimationClip parriedClip00, parriedClip01;
    [SerializeField] bool hasMultipleParries;
    [SerializeField] AnimationCurve damagedMovementCurve;
    float damagedCurveAverage;

    public virtual void OnEnable()
    {
        enemyRefs.enemyEvents.OnReceiveDamage += ReceiveDamage;
        enemyRefs.enemyEvents.OnGettingParried += GettingParried;
        enemyRefs.enemyEvents.OnDeath += slowMoOnDeath;
        damagedCurveAverage = UsefullMethods.GetAverageValueOfCurve(damagedMovementCurve, 10);
    }
    public virtual void OnDisable()
    {
        enemyRefs.enemyEvents.OnReceiveDamage -= ReceiveDamage;
        enemyRefs.enemyEvents.OnGettingParried -= GettingParried;
    }
    public void ReceiveDamage(object sender, ReceivedAttackInfo receivedAttackinfo)
    {
        if(enemyRefs.stateMachine.CurrentState != Enemy_StateMachine.States.Dead)
        {
            enemyRefs.flasher.CallDefaultFlasher();
            TimeScaleEditor.Instance.HitStop(0.05f);
        }

        enemyRefs.agrooMovement.EV_SlowRotationSpeed();
        enemyRefs.agrooMovement.EV_SlowMovingSpeed();
        //enemyMovement.IsAgroo = true;

        enemyRefs.animator.SetTrigger(TagsCollection.PushBack);
        StartCoroutine(WaitReceiveDamage());
        //Vector2 AttackerDirection = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
        //enemyRefs._rigidbody.AddForce(AttackerDirection * receivedAttackinfo.KnockBack);
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(enemyRefs._rigidbody, receivedAttackinfo.ConcreteDirection * receivedAttackinfo.KnockBack ,0.2f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            enemyRefs.characterMover,
            receivedAttackinfo.KnockBack,
            0.25f,
            receivedAttackinfo.ConcreteDirection,
            damagedMovementCurve,
            damagedCurveAverage));
       
    }  
    IEnumerator WaitReceiveDamage()
    {
        yield return new WaitForSeconds(0.3f);
        enemyRefs.agrooMovement.EV_ReturnAllSpeed(0);
    }
    public void GettingParried(int i) // Go to Damage Dealer to set int
    {
        if (!hasMultipleParries)
        {
            enemyRefs.animator.SetTrigger(TagsCollection.HitShield);

            GetComponent<Generic_ShowHideAttackCollider>().HideCollliderOnParry();

            return;
        }

        Enemy_ReusableStateMachine reusableStateMachine = enemyRefs.reusableStateMachine;
        
        if (i == 0) 
        { 
            //Find the 00 clip if null
            if(parriedClip00 == null)
            {
                parriedClip00 = reusableStateMachine.getClipInReplacer(Enemy_ReusableStateMachine.animationStates.BaseEnemy_Parried);
            }

            //Replace the states clip
            reusableStateMachine.ReplaceaStatesClip(
            reusableStateMachine.statesDictionary[Enemy_ReusableStateMachine.animationStates.BaseEnemy_Parried],
            parriedClip00
            );
        }
        else if(i == 1) 
        {
            if (parriedClip01 == null)
            {
                parriedClip01 = reusableStateMachine.getClipInReplacer(Enemy_ReusableStateMachine.animationStates.BaseEnemy_Parried_Extra);
            }

            reusableStateMachine.ReplaceaStatesClip(
            reusableStateMachine.statesDictionary[Enemy_ReusableStateMachine.animationStates.BaseEnemy_Parried_Extra],
            parriedClip01
            );
        }

        enemyRefs.animator.SetTrigger(TagsCollection.HitShield);

        GetComponent<Generic_ShowHideAttackCollider>().HideCollliderOnParry();

    }
    public void EndHitShield()
    {
        enemyRefs.animator.SetBool(TagsCollection.HitShield, false);
    }
    void slowMoOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        TimeScaleEditor.Instance.SlowMotion(SlowMoPercentage.Value, 1f);
    }

}
