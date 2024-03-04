using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_EventSystem;

public class Enemy_StanceMeter : MonoBehaviour
{
    [SerializeField] float MaxStance;
    [SerializeField] float CurrentStance;
    [SerializeField] float CooldownAfterDamage;
    [SerializeField] float CooldownStanceBroken;
    [SerializeField] float RecoveryPerSecond;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Animator animator;
    bool isInFullRecovery;
    bool isRecovering;
    private void OnEnable()
    {
        eventSystem.OnReceiveDamage += RemoveStance;
    }
    private void OnDisable()
    {
        eventSystem.OnReceiveDamage -= RemoveStance;
    }
    private void Start()
    {
        CurrentStance = MaxStance;
    }
    void RemoveStance(object sender, EventArgs_ReceivedAttackInfo receivedAttackInfo)
    {
        if(!isInFullRecovery)
        {
            CurrentStance -= receivedAttackInfo.Damage;
            if (CurrentStance <= 0)
            {
                CurrentStance = 0;
                animator.SetTrigger(TagsCollection.Instance.StanceBroken);
                StartCoroutine(Cooldown(CooldownStanceBroken));
                isInFullRecovery = true;
                if(eventSystem.OnStanceBroken != null) eventSystem.OnStanceBroken();
            }
            else
            {
                StartCoroutine(Cooldown(CooldownAfterDamage));
            }
        }
        
    }
    IEnumerator Cooldown(float cooldownSeconds)
    {
        isRecovering = false;
        yield return new WaitForSeconds(cooldownSeconds);
        isRecovering = true;
    }
    private void Update()
    {
        if(isRecovering)
        {
            CurrentStance += Time.deltaTime * RecoveryPerSecond;
            if (CurrentStance > MaxStance) 
            {
                CurrentStance = MaxStance; 
                isRecovering = false;
                isInFullRecovery = false;
            }
        }
    }
}
