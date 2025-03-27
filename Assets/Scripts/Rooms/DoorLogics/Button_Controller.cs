using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Controller : MonoBehaviour, IDamageReceiver
{
    Animator buttonAnimator;

    private void OnEnable()
    {
        buttonAnimator = GetComponent<Animator>();
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        buttonAnimator.SetTrigger("Hit");
        OnDamageReceived_event?.Invoke(info);
    }
}
