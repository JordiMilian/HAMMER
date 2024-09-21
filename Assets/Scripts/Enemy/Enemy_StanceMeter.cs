using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_EventSystem;

public class Enemy_StanceMeter : MonoBehaviour
{
    [SerializeField] float MaxStance;
    float BaseMaxStance;
    [SerializeField] float CurrentStance;
    [SerializeField] float CooldownAfterDamage;
    [SerializeField] float RecoveryPerSecond;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Animator animator;
    bool isRecovering;

    //Each enemy has some Stance, whenever its damaged, the amount of damage is removed from the Stance meter.
    //There is a cooldown after damaged to slowly recover stance if not damaged for a while
    //If the stance reaches 0 the stance event is called and the animation, which cancels the current attack or action.
    //After breaking the stance the enemy will recover all their stance instantly 

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
    void RemoveStance(object sender, ReceivedAttackInfo receivedAttackInfo)
    {

        CurrentStance -= receivedAttackInfo.Damage;
        if (CurrentStance <= 0)
        {
            StanceBroken();
        }
        else
        {
            StartCoroutine(Cooldown(CooldownAfterDamage));
        }
        
    }
    void StanceBroken()
    {
        CurrentStance = MaxStance; 

        animator.SetTrigger(TagsCollection.StanceBroken);
        if (eventSystem.OnStanceBroken != null) eventSystem.OnStanceBroken();
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
            }
        }
    }
    public void MakeStanceUnbreakeable()
    {
        BaseMaxStance = MaxStance;
        MaxStance = 9999;
        CurrentStance = 9999;
    }
    public void ReturnToRegularStance()
    {
        MaxStance = BaseMaxStance;
        CurrentStance = MaxStance;
    }
}
