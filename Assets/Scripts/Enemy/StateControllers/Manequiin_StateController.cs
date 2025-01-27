using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manequiin_StateController : MonoBehaviour, IParryReceiver, IDamageReceiver
{
    Generic_References genericRefs;
    [SerializeField] Enemy_ShowHideAttackCollider showHideAttackCollider;
    private void OnEnable()
    {
        genericRefs.genericEvents.OnGettingParried += OnGettingParried;
        genericRefs.genericEvents.OnReceiveDamage += OnReceiveDamage;
    }
    public void OnGettingParried( Generic_EventSystem.GettingParriedInfo info)
    {
        genericRefs.animator.SetTrigger(Tags.HitShield);
        showHideAttackCollider.EV_Enemy_HideAttackCollider();
    }
    public void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        genericRefs.animator.SetTrigger(Tags.PushBack);

    }
}
