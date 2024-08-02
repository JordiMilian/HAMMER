using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Enemy_HalfHealthSpecialAttack : MonoBehaviour
{
    [SerializeField] Enemy_References refs;
    [SerializeField] Enemy_AttacksProviderV2.EnemyAttack SpecialAttack;

    [SerializeField] float PercentOfHealthToActivate;

    private void OnEnable()
    {
        refs.enemyEvents.OnReceiveDamage += (object sender, Generic_EventSystem.ReceivedAttackInfo info) => checkIfAttack();
    }
    private void OnDisable()
    {
        refs.enemyEvents.OnReceiveDamage -= (object sender, Generic_EventSystem.ReceivedAttackInfo info) => checkIfAttack();
    }
    void checkIfAttack()
    {
        if(refs.healthSystem.CurrentHP.GetValue() < refs.healthSystem.MaxHP.GetValue() * (PercentOfHealthToActivate/100 ))
        {
            refs.attackProvider.ForcedNextAttack = SpecialAttack;

            refs.enemyEvents.OnReceiveDamage -= (object sender, Generic_EventSystem.ReceivedAttackInfo info) => checkIfAttack();
        }
    }
}
