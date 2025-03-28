using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManequin_Attacker_Controller : MonoBehaviour, IDamageReceiver, IParryReceiver, IDamageDealer
{
    [SerializeField] Animator animator;
    [SerializeField] Generic_StateMachine stateMachine;
    [SerializeField] State ParriedState, IdleState;
    [SerializeField] Collider2D damageDealer;
    [SerializeField] TrailRenderer weaponTrail;
    [SerializeField] AudioClip SFX_Swing;
    private void Start()
    {
        stateMachine.ChangeState(IdleState);
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
    [SerializeField] Generic_Flash flasher;
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("Hit");
        flasher.CallDefaultFlasher();
        OnDamageReceived_event?.Invoke(info);
    }
    public void OnParryReceived(GettingParriedInfo info)
    {
        stateMachine.ChangeState(ParriedState);
        OnParryReceived_event?.Invoke(info);
    }
    public void EV_ShowAttackCollider()
    {
        damageDealer.enabled = true;
        weaponTrail.emitting = true;
        SFX_PlayerSingleton.Instance.playSFX(SFX_Swing, 0.1f);
    }
    public void EV_HideAttackCollider()
    {
        damageDealer.enabled = false;
        weaponTrail.emitting = false;
    }
}
