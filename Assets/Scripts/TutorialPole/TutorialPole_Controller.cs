using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TutorialPole_Controller : MonoBehaviour, IDamageReceiver
{
    [SerializeField] VisualEffect VFX_HitEffect;
    [SerializeField] Button_Controller buttonController;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }

    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("Hurt");

        //The dialogue is controlled by the Dialoguer
        buttonController.OnDamageReceived(info);

        VFX_HitEffect.Play();
        OnDamageReceived_event?.Invoke(info);
    }

}
