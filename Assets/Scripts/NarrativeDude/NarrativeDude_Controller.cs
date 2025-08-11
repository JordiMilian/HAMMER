using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeDude_Controller : MonoBehaviour , IDamageReceiver
{
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("Hit");
        OnDamageReceived_event?.Invoke(info);
    }
}
