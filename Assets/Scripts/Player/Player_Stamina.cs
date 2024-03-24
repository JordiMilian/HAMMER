using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] FloatVariable maxStamina;
    [SerializeField] FloatVariable currentStamina;
    [SerializeField] float RecoveryPerSecond;
    [SerializeField] float DelayToRecover;
    [SerializeField] float DelayToFullRecover;
    bool isRecovering;
    Coroutine currentCooldown;
    private void Start()
    {
        currentStamina.Value = maxStamina.Value;
    }
    private void OnEnable()
    {
        eventSystem.OnStaminaAction += RemoveStamina;
    }
    private void OnDisable()
    {
        eventSystem.OnStaminaAction -= RemoveStamina;
    }
    void RemoveStamina(object sender, Player_EventSystem.EventArgs_StaminaConsumption args)
    {
        //Remove Stamina
        currentStamina.Value -= args.StaminaUsage; 

        //Set to max or min in case of overflow
        if(currentStamina.Value < 0 ) { currentStamina.Value = 0; }
        if(currentStamina.Value > maxStamina.Value ) { currentStamina.Value = maxStamina.Value; }

        //Stop recovering and stop cooldown if still ON
        isRecovering = false; 
        if(currentCooldown != null) { StopCoroutine(currentCooldown); }

        //New Cooldown depending on stamina
        if(currentStamina.Value <= 0) { currentCooldown = StartCoroutine(CooldownToRecover(DelayToFullRecover)); }
        else { currentCooldown = StartCoroutine(CooldownToRecover(DelayToRecover)); }
        
    }
    private void Update()
    {
        if (isRecovering)
        {
            currentStamina.Value += Time.deltaTime * RecoveryPerSecond;
            if (currentStamina.Value > maxStamina.Value)
            {
                currentStamina.Value = maxStamina.Value;
                isRecovering = false;
            }
        }
    }
    IEnumerator CooldownToRecover(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isRecovering = true;
    }
}
