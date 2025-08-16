using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{
    public bool isRecovering { get; private set; }
    public bool isFilled{ get; private set; }
    public bool isEmpty { get; private set; }

    [SerializeField] Player_References playerRefs;
    [SerializeField] float RecoveryPerSecond; //This should be in Stats probably
    private void Start()
    {
        playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
        isFilled = true;
        isEmpty = false;
    }
    private void OnEnable()
    {
        playerRefs.currentStats.OnStaminaChange += onMaxStaminaUpdated;
    }
    private void OnDisable()
    {
        playerRefs.currentStats.OnStaminaChange -= onMaxStaminaUpdated;
    }
    public void RemoveStamina(float stamina)
    {
        //Remove Stamina
        playerRefs.currentStats.CurrentStamina -= stamina;

        //Set to max or min in case of overflow
        if (playerRefs.currentStats.CurrentStamina <= 0) { playerRefs.currentStats.CurrentStamina = 0; isEmpty = true; }
        else { isEmpty = false; }

        if (playerRefs.currentStats.CurrentStamina > playerRefs.currentStats.MaxStamina)
        {
            playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
            isFilled = true;
            return;
        }

        isFilled = false;
        isRecovering = false;
    }
    public void StartRecovering()
    {
        isRecovering = true;
    }
    public void StopRecovering()
    {
        isRecovering = false;
    }
    public void FillStamina()
    {
        playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;

        isFilled = true;
        isRecovering = false;
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
  
    private void Update()
    {
        if (isRecovering)
        {
            playerRefs.currentStats.CurrentStamina += Time.deltaTime * RecoveryPerSecond * playerRefs.currentStats.RecoveryStaminaSpeed;
            isEmpty = false;

            if (playerRefs.currentStats.CurrentStamina > playerRefs.currentStats.MaxStamina)
            {
                playerRefs.currentStats.CurrentStamina = playerRefs.currentStats.MaxStamina;
                isRecovering = false;
                isFilled = true;
            }
            
        }
    }
}
