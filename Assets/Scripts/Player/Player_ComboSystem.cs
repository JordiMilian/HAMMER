using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using System.Diagnostics;
using UnityEngine;

public class Player_ComboSystem : MonoBehaviour
{
    [SerializeField] float CheckIfHoldTime = 0.2f;
    [SerializeField] float AttackTime = 0.2f;
    [SerializeField] float BaseDamage;
    [SerializeField] bool Attack01Cued;
    [SerializeField] bool Attack02Cued;
    [SerializeField] bool IsWaitingAttack;
    public bool isAttacking;

    private enum ComboState
    {
        Attackin01, Attackin02, NoAttacking, TotalRecovery01, TotalRecovery02, Charging01,Charging02, ActuallyCharging01, ActuallyCharging02, 
    }
    [SerializeField] ComboState comboState = ComboState.NoAttacking;
    Animator animator;
    //bool Attacking01, Attackin02, NoAttacking;

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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(comboState);
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (IsWaitingAttack)
            {
                switch (comboState)
                {

                }
            }
            if(!IsWaitingAttack)
            {

            }
            switch (comboState)
            {
                default:
                    Debug.Log("¿?");
                    break;
                case ComboState.Attackin02:
                    Attack01Cued = true;
                    WaitForAttackToEnd(ComboState.Charging01); 

                    break;
                case ComboState.Attackin01:
                    Attack02Cued = true;
                    WaitForAttackToEnd(ComboState.Charging02);
                    break;
                case ComboState.NoAttacking:
                    comboState = ComboState.Charging01;
                    break;
            }

            StartCoroutine(CheckIfHold());
            
            
        }
     if (Input.GetKeyUp(KeyCode.N))
        {
                if (!isAttacking)
                {
                 switch(comboState)
                    {
                        case ComboState.ActuallyCharging01:
                            StartCoroutine(AttackSimulation(ComboState.Attackin01));
                            break;
                        case ComboState.ActuallyCharging02:
                            StartCoroutine(AttackSimulation(ComboState.Attackin02));
                            break;
                        case ComboState.Charging01:
                            StartCoroutine(AttackSimulation(ComboState.Attackin01));
                            break;
                        case ComboState.Charging02:
                            StartCoroutine(AttackSimulation(ComboState.Attackin02));
                            break;
                        default:
                            Debug.Log("¿?¿");
                            break;
                    }
               
                }
  
        }
    }
  IEnumerator AttackSimulation(ComboState attackState)
    {
        isAttacking = true;
        comboState = attackState;

        IsWaitingAttack = true;
        float timer = 0; 
            while (timer < AttackTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if(!Attack01Cued && !Attack02Cued) { comboState = ComboState.NoAttacking; }
        
            IsWaitingAttack = false;
            isAttacking = false;
    }
    IEnumerator WaitForAttackToEnd(ComboState state)
    {
        while (IsWaitingAttack)
        {
            yield return null;
        }
        if (state == ComboState.Charging01) { Attack01Cued = false; }
        if (state == ComboState.Charging02) { Attack02Cued = false; }
        comboState = state;
    }

    IEnumerator CheckIfHold()
    {
        float timer = 0;
        while (Input.GetKey(KeyCode.N))
        {
            timer += Time.deltaTime;
            if(timer > CheckIfHoldTime)
            {
                CheckCharging(); //Change to ActuallyCharging depending on which charge
            }
            yield return null;
        }   
    }
    void CheckCharging()
    {
        if (comboState == ComboState.Charging01) { comboState = ComboState.ActuallyCharging01; }
        if (comboState == ComboState.Charging02) { comboState = ComboState.ActuallyCharging02; }
    }
}

