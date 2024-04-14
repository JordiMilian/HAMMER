using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    [SerializeField] float RecoveryPerSecond;
    [SerializeField] float DelayToRecover;
    [SerializeField] float DelayToFullRecover;
    bool isRecovering;
    Coroutine currentCooldown;
    private void Start()
    {
        playerRefs.currentStamina.Value = playerRefs.maxStamina.Value;
    }
    private void OnEnable()
    {
        playerRefs.playerEvents.OnStaminaAction += RemoveStamina;
    }
    private void OnDisable()
    {
        playerRefs.playerEvents.OnStaminaAction -= RemoveStamina;
    }
    void RemoveStamina(float stamina)
    {
        //Remove Stamina
        playerRefs.currentStamina.Value -= stamina; 

        //Set to max or min in case of overflow
        if (playerRefs.currentStamina.Value < 0 ) { playerRefs.currentStamina.Value = 0; }
        if (playerRefs.currentStamina.Value > playerRefs.maxStamina.Value ) { playerRefs.currentStamina.Value = playerRefs.maxStamina.Value; }

        //Stop recovering and stop cooldown if still ON
        isRecovering = false; 
        if(currentCooldown != null) { StopCoroutine(currentCooldown); }

        //New Cooldown depending on stamina
        if (playerRefs.currentStamina.Value <= 0) { currentCooldown = StartCoroutine(CooldownToRecover(DelayToFullRecover)); }
        else { currentCooldown = StartCoroutine(CooldownToRecover(DelayToRecover)); }
        
    }
    private void Update()
    {
        if (isRecovering)
        {
            playerRefs.currentStamina.Value += Time.deltaTime * RecoveryPerSecond;
            if (playerRefs.currentStamina.Value > playerRefs.maxStamina.Value)
            {
                playerRefs.currentStamina.Value = playerRefs.maxStamina.Value;
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
