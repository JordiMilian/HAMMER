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
    string AttacksCountTag = "AttacksCount";
   

    [SerializeField] Animator animator;
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] Collider2D weaponDamageCollider;
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_Stats stats;

    public bool canAttack;
    public int attacksCount;

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
        AddToCount(1);
        SetdamageDealer();
    }
    void AddToCount(int add)
    {
        attacksCount += add;
        if(attacksCount > 3) { attacksCount = 3; }
        if(attacksCount < 0) { attacksCount = 0; }
        animator.SetInteger(AttacksCountTag,attacksCount);
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
    public void EV_RemoveCount()
    {
        AddToCount(-1);
    }
}

