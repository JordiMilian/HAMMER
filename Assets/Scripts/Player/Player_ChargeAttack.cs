using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ChargeAttack : MonoBehaviour
{
    public bool isCharging;
    [SerializeField] float DamageAdded;
    [SerializeField] float MaxDamage;
    Player_Controller playerController;
    float Adder;
    void Start()
    {
        playerController = GetComponent<Player_Controller>();
    }
    void FixedUpdate()
    {
        if (isCharging) { Charging(); }   
    }
    public void OnStartCharge() { isCharging = true; Adder = 0; }

    void Charging()
    {
        if(playerController.CurrentDamage < MaxDamage)
        {
             Adder += Time.deltaTime * DamageAdded;
            playerController.CurrentDamage = Mathf.Lerp(playerController.BaseDamage, MaxDamage, Adder);  
        }

        if(playerController.CurrentDamage > MaxDamage) 
        {
            playerController.CurrentDamage = MaxDamage;
            isCharging = false;
        }
    }
}
