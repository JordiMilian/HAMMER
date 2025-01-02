using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AttackedWhileRecovering : MonoBehaviour
{
    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] int ResponseAttackIndex;
    [SerializeField] float CooldownSeconds = 2;
    bool isInRecovery;
    bool isInCooldown;
    private void OnEnable()
    {
        enemyRefs.enemyEvents.OnReceiveDamage += PerformResponse;
        enemyRefs.enemyEvents.OnAttackFinished += RecoveryEnded;
    }
    private void OnDisable()
    {
        enemyRefs.enemyEvents.OnReceiveDamage -= PerformResponse;
        enemyRefs.enemyEvents.OnAttackFinished -= RecoveryEnded;
    }
    public void EV_StartRecovery()
    {
        isInRecovery = true;
    }
    void RecoveryEnded()
    {
        isInRecovery = false;
    }
    //Este script no se si esta deprecated o que
    void PerformResponse(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        if(isInRecovery && !isInCooldown)
        {
            enemyRefs.animator.SetTrigger(Tags.AttackedWhileRecovering); //Set animation trigger
            enemyRefs.attackProvider.PerformAttack(enemyRefs.attackProvider.Enemy_Attacks[ResponseAttackIndex]); //Perform attack from provider
            //enemyRefs.animator.SetBool(enemyRefs.attackProvider.Enemy_Attacks[ResponseAttackIndex].TriggerName, false); //Uncheck trigger checked by the provider
            StartCoroutine(Cooldown()); //Cooldown
        }
    }
    IEnumerator Cooldown()
    {
        isInCooldown = true;
        float waitTime = CooldownSeconds + enemyRefs.attackProvider.Enemy_Attacks[ResponseAttackIndex].animationClip.length;
        yield return new WaitForSeconds(waitTime);
        isInCooldown = false;
    }
}
