using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class dialoguePole_animator : MonoBehaviour, IDamageReceiver
{
    [SerializeField] Animator animator;
    [SerializeField] VisualEffect breakEffect;

   
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("hit");
        breakEffect.SetFloat("normalizedIntensity", 0.35f);
        breakEffect.Play();
    }
}
