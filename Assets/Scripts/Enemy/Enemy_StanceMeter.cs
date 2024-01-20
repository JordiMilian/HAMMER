using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_DamageDetector;

public class Enemy_StanceMeter : MonoBehaviour
{
    [SerializeField] float MaxStance;
    [SerializeField] float CurrentStance;
    [SerializeField] float CooldownAfterDamage;
    [SerializeField] float CooldownStanceBroken;
    [SerializeField] float RecoveryPerSecond;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] Animator animator;
    bool isInFullRecovery;
    bool isRecovering;
    public event EventHandler onStanceBroken;
    private void OnEnable()
    {
        damageDetector.OnReceiveDamage += RemoveStance;
    }
    private void OnDisable()
    {
        damageDetector.OnReceiveDamage -= RemoveStance;
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
                Debug.Log("stance broken");
                animator.SetTrigger(TagsCollection.Instance.StanceBroken);
                StartCoroutine(Cooldown(CooldownStanceBroken));
                isInFullRecovery = true;
                if(onStanceBroken != null) onStanceBroken(this, EventArgs.Empty);
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
