using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AttackedWhileRecovering : MonoBehaviour
{
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Generic_EventSystem eventSystem;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;
    [SerializeField] int ResponseAttackIndex;
    [SerializeField] float CooldownSeconds = 2;
    bool isInRecovery;
    bool isInCooldown;
    private void OnEnable()
    {
        eventSystem.OnReceiveDamage += PerformResponse;
        eventSystem.OnAttackFinished += RecoveryEnded;
    }
    private void OnDisable()
    {
        eventSystem.OnReceiveDamage -= PerformResponse;
        eventSystem.OnAttackFinished -= RecoveryEnded;
    }
    public void EV_StartRecovery()
    {
        isInRecovery = true;
    }
    void RecoveryEnded()
    {
        isInRecovery = false;
    }
    void PerformResponse(object sender, Generic_EventSystem.EventArgs_ReceivedAttackInfo args)
    {
        if(isInRecovery && !isInCooldown)
        {
            Debug.Log("Response ON");
            enemyAnimator.SetTrigger(TagsCollection.Instance.AttackedWhileRecovering); //Set animation trigger
            attackProvider.PerformAttack(attackProvider.Enemy_Attacks[ResponseAttackIndex]); //Perform attack from provider
            enemyAnimator.SetBool(attackProvider.Enemy_Attacks[ResponseAttackIndex].TriggerName, false); //Uncheck trigger checked by the provider
            StartCoroutine(Cooldown()); //Cooldown
        }
    }
    IEnumerator Cooldown()
    {
        isInCooldown = true;
        float waitTime = CooldownSeconds + attackProvider.Enemy_Attacks[ResponseAttackIndex].animationClip.length;
        yield return new WaitForSeconds(waitTime);
        isInCooldown = false;
    }
}
