using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HalfHealthSpecialAttack : MonoBehaviour
{/*
    [SerializeField] Enemy_References refs;
    [SerializeField] Enemy_AttacksProviderV2.EnemyAttack SpecialAttack;
    float BaseMaxStance;

    [SerializeField] float PercentOfHealthToActivate;
    public Action OnChangePhase;

    private void OnEnable()
    {
        refs.enemyEvents.OnReceiveDamage += checkIfAttack;
    }
    private void OnDisable()
    {
        refs.enemyEvents.OnReceiveDamage -= checkIfAttack;
    }
    void checkIfAttack(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        //Debug.Log("Current Health: " + refs.healthSystem.CurrentHP.GetValue() + "Desired Health: " + refs.healthSystem.MaxHP.GetValue() * (PercentOfHealthToActivate / 100));
        if (refs.currentEnemyStats.CurrentHp < refs.currentEnemyStats.MaxHp * (PercentOfHealthToActivate/100 ))
        {
            refs.stateController.ForceNextAttack(SpecialAttack);

            refs.enemyEvents.OnReceiveDamage -=  checkIfAttack;

            refs.enemyEvents.OnStartAttack += ActuallyPerformChangePhase;
        }
    }
    void ActuallyPerformChangePhase()
    {
        refs.enemyEvents.OnStartAttack -= ActuallyPerformChangePhase;

        refs.stanceMeter.MakeStanceUnbreakeable();
        OnChangePhase?.Invoke();

        refs.enemyEvents.OnAttackFinished += EndedChangePerform;
    }
    void EndedChangePerform()
    {
        refs.enemyEvents.OnAttackFinished -= EndedChangePerform;
        refs.stanceMeter.ReturnToRegularStance();
    }
    */
}
