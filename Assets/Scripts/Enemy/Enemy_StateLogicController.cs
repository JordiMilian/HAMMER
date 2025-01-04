using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageReceiver
{
    public void ReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info);
}
public class Enemy_StateLogicController : MonoBehaviour, IDamageReceiver
{
    [SerializeField] Enemy_References enemyRefs;
    Enemy_EventSystem enemyEvents;
    private void OnEnable()
    {
        enemyEvents = enemyRefs.enemyEvents;
        enemyEvents.OnReceiveDamage += ReceiveDamage;
        enemyEvents.OnGettingParried += GotParried;

    }
    public void ReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        enemyRefs.animator.SetTrigger(Tags.PushBack);
    }
    void GotParried(int i)
    {
        enemyRefs.animator.SetTrigger(Tags.HitShield);
    }
}
