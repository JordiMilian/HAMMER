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
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] GameObject FollowMouse;
    [SerializeField] Player_VFXManager VFXManager;
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] FloatVariable CurrentStamina;

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
        if(CurrentStamina.Value > 0)
        {
            eventSystem.OnStaminaAction?.Invoke(this, new Generic_EventSystem.EventArgs_StaminaConsumption(2));
            playerMovement.canDash = false;
            AddToCount(1);
            SetdamageDealer();
        } 
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
    public void EV_ShowWeaponCollider() { weaponDamageCollider.enabled = true; VFXManager.EV_ShowTrail(); }
    public void EV_HideWeaponCollider() { weaponDamageCollider.enabled = false; VFXManager.EV_HideTrail(); }
    public void EV_AddForce(float force)
    {
        //Make equivalent between min and max distance to -0,5 and 1
        float inverseLerpedDistance = Mathf.InverseLerp(0.2f, 3f, distanceToEnemy.Value);
        float lerpedDistance = Mathf.Lerp(-0.5f, 1, inverseLerpedDistance);
        if(distanceToEnemy.Value > 4) { lerpedDistance = 0.5f; } //If the player is too far, behave normally (normally is at 2 now)
        Vector3 tempForce = FollowMouse.gameObject.transform.up * force * lerpedDistance;
        StartCoroutine(ApplyForceOverTime(tempForce, 0.1f));

         
    }
    public void EV_RemoveCount()
    {
        AddToCount(-1);
    }
    public void ResetCount()
    {
        attacksCount = 0;
        animator.SetInteger(AttacksCountTag, attacksCount);
    }
    IEnumerator ApplyForceOverTime(Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            playerRigidbody.AddForce(forceVector / duration);
            yield return null;
        }
    }
}

