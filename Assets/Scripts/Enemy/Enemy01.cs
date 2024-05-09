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
    


    public virtual void OnEnable()
    {
        enemyRefs.enemyEvents.OnReceiveDamage += ReceiveDamage;
        enemyRefs.enemyEvents.OnGettingParried += GettingParried;
        enemyRefs.enemyEvents.OnDeath += slowMoOnDeath;
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
            enemyRefs.flasher.CallFlasher();
            TimeScaleEditor.Instance.HitStop(0.05f);
        }

        enemyRefs.agrooMovement.EV_SlowRotationSpeed();
        enemyRefs.agrooMovement.EV_SlowMovingSpeed();
        //enemyMovement.IsAgroo = true;

        enemyRefs.animator.SetTrigger(TagsCollection.PushBack);
        StartCoroutine(WaitReceiveDamage());
        Vector2 AttackerDirection = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
        StartCoroutine(UsefullMethods.ApplyForceOverTime(enemyRefs._rigidbody, AttackerDirection * receivedAttackinfo.KnockBack ,0.1f));
       
    }  
    IEnumerator WaitReceiveDamage()
    {
        yield return new WaitForSeconds(0.3f);
        enemyRefs.agrooMovement.EV_ReturnAllSpeed(0);
    }
    public void GettingParried(int i)
    {
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
