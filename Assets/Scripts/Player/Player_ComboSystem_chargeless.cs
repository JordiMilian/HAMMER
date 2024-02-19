using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_chargeless : MonoBehaviour
{
    public float CurrentDamage;
    public float BaseDamage;

    //string Attack01_Charging = "Attack01_Charging";
    //string Attack02_Charging = "Attack02_Charging";
    string Attack01_Release = "Attack01";
    string Attack02_Release = "Attack02";
   
    public enum NextAttack { NextAttack01, NextAttack02,}
    public NextAttack nextAttack = NextAttack.NextAttack01;

    [SerializeField] Animator animator;
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] Collider2D weaponDamageCollider;
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_Stats stats;

    //public bool isCurrentAttackCanceled;
    public bool canAttack;

    /*
    [Header("CHARGING")]
    bool isCharging;
    float MaxDamage;
    float DamageAdded;
    float Adder;
    */

    void Start()
    {
        animator = GetComponent<Animator>();
        canAttack = true;
        playerMovement = GetComponent<Player_Movement>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(canAttack)
            {
                SetReleaseTriggers();
            }

            if (!canAttack)
            {
                StartCoroutine(WaitForCanAttackRelease());
            }
        }
    }
    
    void SetReleaseTriggers()
    {
        playerMovement.canDash = false;
        switch (nextAttack)
        {
            case NextAttack.NextAttack01:
                if (!animator.GetBool(Attack01_Release))
                {
                    animator.SetTrigger(Attack01_Release);
                    nextAttack = NextAttack.NextAttack02;
                }
                break;

            case NextAttack.NextAttack02:
                if (!animator.GetBool(Attack02_Release))
                {
                    animator.SetTrigger(Attack02_Release);
                    nextAttack = NextAttack.NextAttack01;
                }
                break;
        }
        SetdamageDealer();
    }
    IEnumerator WaitForCanAttackRelease()
    {
        while (!canAttack)
        {
            yield return null;
        }
        SetReleaseTriggers();
    }


    void SetdamageDealer()
    {
        damageDealer.Damage = CurrentDamage * stats.DamageMultiplier;
    }
    
    public void ComboOver()
    {
        nextAttack = NextAttack.NextAttack01;
    }
    public void CanAttack()
    {
        canAttack = true;
    }
    public void EV_ShowWeaponCollider() { weaponDamageCollider.enabled = true; }
    public void EV_HideWeaponCollider() { weaponDamageCollider.enabled = false; }
    public void EV_AddForce(float force)
    {
        playerRigidbody.AddForce(weaponDamageCollider.gameObject.transform.up * force);
    }

}

