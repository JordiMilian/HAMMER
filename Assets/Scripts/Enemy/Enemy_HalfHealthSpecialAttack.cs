using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HalfHealthSpecialAttack : MonoBehaviour
{
    [SerializeField] Enemy_References refs;
    [SerializeField] Enemy_AttacksProviderV2.EnemyAttack SpecialAttack;

    [SerializeField] float PercentOfHealthToActivate;

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
        if (refs.healthSystem.CurrentHP.GetValue() < refs.healthSystem.MaxHP.GetValue() * (PercentOfHealthToActivate/100 ))
        {
            refs.attackProvider.ForceNextAttack(SpecialAttack);
            refs.enemyEvents.OnReceiveDamage -=  checkIfAttack;
        }
    }
}
