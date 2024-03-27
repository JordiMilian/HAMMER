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
    
    [SerializeField] GameObject FollowMouse;
    [SerializeField] Player_VFXManager VFXManager;
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] FloatVariable CurrentStamina;
    [SerializeField] Player_ActionPerformer actionPerformer;

    [Header("Adapt to distance to Enemy")]
    [SerializeField] float Force;
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] FloatVariable defaultDistance;
    [SerializeField] float minDistance = 0.2f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] float minForce = - 0.5f;
    [SerializeField] float maxForce = 1f;

    public bool canAttack;
    public int attacksCount;

    void Start()
    {
        animator = GetComponent<Animator>();
        canAttack = true;
        playerMovement = GetComponent<Player_Movement>();
    }
    private void OnEnable()
    {
        eventSystem.OnPerformAttack += RemoveAttackStamina;
    }
    private void OnDisable()
    {
        eventSystem.OnPerformAttack -= RemoveAttackStamina;
    }
    void RemoveAttackStamina()
    {
        eventSystem.OnStaminaAction?.Invoke(this, new Player_EventSystem.EventArgs_StaminaConsumption(2));
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(CurrentStamina.Value > 0)
            {
               actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Attack"));
            }
            

            if(canAttack)
            {
                //SetReleaseTriggers();
            }

            if (!canAttack)
            {
                //StartCoroutine(WaitForCanAttackRelease());
            }
        }
    }
    
    void SetReleaseTriggers()
    {
        if(CurrentStamina.Value > 0)
        {
            eventSystem.OnStaminaAction?.Invoke(this, new Player_EventSystem.EventArgs_StaminaConsumption(2));
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
    public void EV_AddForce()
    {
        //Make equivalent between min and max distance to -0,5 and 1
        float equivalent;
        if(distanceToEnemy.Value > maxDistance) { equivalent = CalculateEquivalent(defaultDistance.Value); } //If the player is too far, behave with default 
        else { equivalent = CalculateEquivalent(distanceToEnemy.Value);} // Else calculate with distance

        Vector3 tempForce = FollowMouse.gameObject.transform.up * Force * equivalent;
        StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRigidbody, tempForce, 0.1f));
    }
    float CalculateEquivalent(float Distance)
    {
        float inverseF = Mathf.InverseLerp(minDistance, maxDistance, Distance);
        float lerpF = Mathf.Lerp(minForce, maxForce, inverseF);
        return lerpF;
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
}

