using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using System.Diagnostics;
using UnityEngine;

public class Player_ComboSystem : MonoBehaviour
{
    [SerializeField] float CheckIfHoldTime = 0.2f;
    [SerializeField] float CurrentDamage;
    [SerializeField] float BaseDamage;
    private enum ComboState
    {
        Attackin01, Attackin02, NoAttacking, TotalRecovery01, TotalRecovery02, Charging01,Charging02,
    }
    [SerializeField] ComboState comboState = ComboState.NoAttacking;
    Animator animator;
    //bool Attacking01, Attackin02, NoAttacking;

    bool ActuallyHolding;
    bool Mouse0Down;
    class Attack
    {
        public float Damage;
        public float MinDamage;
        public bool IsCharge;
        public bool IsWaiting;

    }
    void Start()
    {
        CurrentDamage = BaseDamage;
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.N))
        {
            Mouse0Down = true;
            StartCoroutine(CheckIfHold());
            
            switch(comboState)
            {
                default:
                    Debug.Log("¿?");
                    break;
                case ComboState.Attackin02:
                    comboState = ComboState.Charging01;
                    break;
                case ComboState.Attackin01:
                    comboState = ComboState.Charging02;
                    break;
                case ComboState.NoAttacking:
                    comboState = ComboState.Charging01;
                    break;
            }
            Debug.Log(comboState);
                

        }
     if (Input.GetKeyUp(KeyCode.N))
        {
            Mouse0Down = false;
            switch(comboState)
            {
                case ComboState.Charging01:
                    comboState = ComboState.Attackin01;
                    break;
                case ComboState.Charging02:
                    comboState = ComboState.Attackin02;
                    break;
                default:
                    Debug.Log("¿?");
                    break;
            }
            Debug.Log(comboState);
            StartCoroutine(AttackSimulation());
        }


     if (ActuallyHolding)
        {
            CurrentDamage += 0.1f;
        }
    }
  IEnumerator AttackSimulation()
    {
        float timer = 0;
       
        while (Input.GetKey(KeyCode.N) == false)
        {
            
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    comboState = ComboState.NoAttacking;
                    Debug.Log(comboState);
                    ActuallyHolding = true;
                    
                }
                yield return null;
            
           
        }
        
    }
    IEnumerator CheckIfHold()
    {
        float timer = 0;
        while (Input.GetKey(KeyCode.N))
        {
            timer += Time.deltaTime;
            if(timer > CheckIfHoldTime)
            {
                Debug.Log("Holded");
                ActuallyHolding = true;
            }
            yield return null;
        }
       
        
    }
    
}
