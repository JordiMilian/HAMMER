using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{
    public bool isRecovering;
    public bool isFilled;

    [SerializeField] Player_References playerRefs;

    [SerializeField] float RecoveryPerSecond;
    [SerializeField] float DelayToRecover;
    
    Coroutine currentCooldown;
    private void Start()
    {
        playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
        isFilled = true;
    }
    private void OnEnable()
    {
        playerRefs.events.CallStaminaAction += RemoveStamina;
        playerRefs.currentStats.OnStaminaChange += onMaxStaminaUpdated;
        playerRefs.events.OnEnterIdle += onReturnToIdle;
    }
    private void OnDisable()
    {
        playerRefs.events.CallStaminaAction -= RemoveStamina;
        playerRefs.currentStats.OnStaminaChange -= onMaxStaminaUpdated;
        playerRefs.events.OnEnterIdle -= onReturnToIdle;
    }
    void onMaxStaminaUpdated(float newValue)
    {
        if(playerRefs.currentStats.MaxStamina > playerRefs.currentStats.CurrentStamina)
        {
            isRecovering = true;
            isFilled = false;
        }
        else if(playerRefs.currentStats.MaxStamina < playerRefs.currentStats.CurrentStamina)
        {
            playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
        }
    }
    void RemoveStamina(float stamina)
    {
        //Remove Stamina
        playerRefs.currentStats.CurrentStamina -= stamina; 

        //Set to max or min in case of overflow
        if (playerRefs.currentStats.CurrentStamina < 0 ) { playerRefs.currentStats.CurrentStamina = 0; }
        else if (playerRefs.currentStats.CurrentStamina > playerRefs.currentStats.MaxStamina) 
        { 
            playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina; 
            isFilled = true; 
            return; 
        }

        isFilled = false;
        //Stop recovering and stop cooldown if still ON
        isRecovering = false; 
        if(currentCooldown != null) { StopCoroutine(currentCooldown); }

        /*
        //New Cooldown depending on stamina
        if (playerRefs.currentStamina.Value <= 0) { currentCooldown = StartCoroutine(CooldownToRecover(DelayToFullRecover)); }
        else { currentCooldown = StartCoroutine(CooldownToRecover(DelayToRecover)); }
        */
    }
    private void Update()
    {
        if (isRecovering)
        {
            playerRefs.currentStats.CurrentStamina += Time.deltaTime * RecoveryPerSecond * playerRefs.currentStats.RecoveryStaminaSpeed;

            if (playerRefs.currentStats.CurrentStamina > playerRefs.currentStats.MaxStamina)
            {
                playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
                isRecovering = false;
                isFilled = true;
            }
        }
    }
    IEnumerator CooldownToRecover(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isRecovering = true;
    }
    void onReturnToIdle()
    {
        currentCooldown = StartCoroutine(CooldownToRecover(DelayToRecover));
    }
}
