using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_chargeless : MonoBehaviour
{
    public float CurrentDamage;
    public float BaseDamage;

    string AttacksCountTag = "AttacksCount";

    [SerializeField] Player_References playerRefs;

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
        canAttack = true;
    }
    private void OnEnable()
    {
        playerRefs.playerEvents.OnPerformAttack += RemoveAttackStamina;
    }
    private void OnDisable()
    {
        playerRefs.playerEvents.OnPerformAttack -= RemoveAttackStamina;
    }
    void RemoveAttackStamina()
    {
        playerRefs.playerEvents.OnStaminaAction?.Invoke(2);
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(playerRefs.currentStamina.Value > 0)
            {
                playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Attack"));
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
        if(playerRefs.currentStamina.Value > 0)
        {
            playerRefs.playerEvents.OnStaminaAction?.Invoke(2);
            playerRefs.playerMovement.canDash = false;
            AddToCount(1);
            SetdamageDealer();
        } 
    }
    void AddToCount(int add)
    {
        attacksCount += add;
        if(attacksCount > 3) { attacksCount = 3; }
        if(attacksCount < 0) { attacksCount = 0; }
        playerRefs.playerAnimator.SetInteger(AttacksCountTag,attacksCount);
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
        playerRefs.damageDealer.Damage = CurrentDamage * playerRefs.playerStats.DamageMultiplier;
    }
    public void CanAttack()
    {
        canAttack = true;
    }
    public void EV_ShowWeaponCollider() { playerRefs.weaponCollider.enabled = true; playerRefs.playerVFX.EV_ShowTrail(); }
    public void EV_HideWeaponCollider() { playerRefs.weaponCollider.enabled = false; playerRefs.playerVFX.EV_HideTrail(); }
    public void EV_AddForce()
    {
        //Make equivalent between min and max distance to -0,5 and 1
        float equivalent;
        if(distanceToEnemy.Value > maxDistance) { equivalent = CalculateEquivalent(defaultDistance.Value); } //If the player is too far, behave with default 
        else { equivalent = CalculateEquivalent(distanceToEnemy.Value);} // Else calculate with distance

        Vector3 tempForce = playerRefs.followMouse.gameObject.transform.up * Force * equivalent;
        StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs.playerRB, tempForce, 0.1f));
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
        playerRefs.playerAnimator.SetInteger(AttacksCountTag, attacksCount);
    }
}

