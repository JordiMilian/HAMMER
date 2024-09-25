using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{
    public bool isRecovering;
    public bool isFilled;
    public float RecoverSpeedMultiplier = 1;

    [SerializeField] Player_References playerRefs;

    [SerializeField] float RecoveryPerSecond;
    [SerializeField] float DelayToRecover;
    
    Coroutine currentCooldown;
    private void Start()
    {
        playerRefs.currentStamina.Value = playerRefs.maxStamina.Value;
        isFilled = true;
    }
    private void OnEnable()
    {
        playerRefs.events.CallStaminaAction += RemoveStamina;
        playerRefs.maxStamina.OnValueSet += checkForMaxStamina;
        playerRefs.baseStamina.SetValue(playerRefs.maxStamina.GetValue());
        playerRefs.events.OnEnterIdle += onReturnToIdle;
    }
    private void OnDisable()
    {
        playerRefs.events.CallStaminaAction -= RemoveStamina;
        playerRefs.maxStamina.OnValueSet -= checkForMaxStamina;
        playerRefs.events.OnEnterIdle -= onReturnToIdle;
    }
    void checkForMaxStamina()
    {
        if(playerRefs.maxStamina.GetValue() > playerRefs.currentStamina.GetValue())
        {
            isRecovering = true;
            isFilled = false;
        }
        else if(playerRefs.maxStamina.GetValue() < playerRefs.currentStamina.GetValue())
        {
            playerRefs.currentStamina.SetValue(playerRefs.maxStamina.GetValue());
        }
    }
    void RemoveStamina(float stamina)
    {
        //Remove Stamina
        playerRefs.currentStamina.Value -= stamina; 

        //Set to max or min in case of overflow
        if (playerRefs.currentStamina.Value < 0 ) { playerRefs.currentStamina.Value = 0; }
        else if (playerRefs.currentStamina.Value > playerRefs.maxStamina.Value ) { playerRefs.currentStamina.Value = playerRefs.maxStamina.Value; isFilled = true; return; }

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
            playerRefs.currentStamina.Value += Time.deltaTime * RecoveryPerSecond * RecoverSpeedMultiplier;

            if (playerRefs.currentStamina.Value > playerRefs.maxStamina.Value)
            {
                playerRefs.currentStamina.Value = playerRefs.maxStamina.Value;
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
