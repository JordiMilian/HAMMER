using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_chargeless : MonoBehaviour
{

    [SerializeField] Player_References playerRefs;

    [Header("Adapt to distance to Enemy")]
    [SerializeField] float Force;
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] FloatVariable defaultDistance;
    public float minDistance = 0.2f;
    public float maxDistance = 3f;
    [SerializeField] float minForce = -0.5f;
    [SerializeField] float maxForce = 1f;
    [SerializeField] AnimationCurve attackCurve;
    [SerializeField] float addForceTime;

    //This is set from the weapon info holder
    [HideInInspector] public float Base_Damage;
    [HideInInspector] public float Base_Knockback;
    [HideInInspector] public float Base_HitStop;
    [HideInInspector] public float StaminaUse;


    private void OnEnable()
    {
        playerRefs.events.OnAttackStarted += onPerformedAttack;
        playerRefs.events.OnEnterIdle += EV_HideWeaponCollider;
        InputDetector.Instance.OnAttackPressed += onAttackPressed;
        
    }
    private void OnDisable()
    {
        playerRefs.events.OnAttackStarted -= onPerformedAttack;
        playerRefs.events.OnEnterIdle -= EV_HideWeaponCollider;
        InputDetector.Instance.OnAttackPressed -= onAttackPressed;
    }

    void onAttackPressed()
    {
        if (playerRefs.currentStamina.Value > 0)
        {
            playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Attack"));
        }
    }
    void onPerformedAttack()
    {
        playerRefs.events.CallStaminaAction?.Invoke(StaminaUse); //Remove stamina

        foreach(Generic_DamageDealer dealer in playerRefs.DamageDealersList) //Get the damage dealers back to base stats
        {
            dealer.Damage = Base_Damage * playerRefs.stats.DamageMultiplier;
            dealer.HitStop = Base_HitStop;
            dealer.Knockback = Base_Knockback;
            dealer.isChargingSpecialAttack = true;
        }

    }
    public void EV_ShowWeaponCollider() { playerRefs.weaponCollider.enabled = true; playerRefs.playerVFX.EV_ShowTrail(); }
    public void EV_HideWeaponCollider() { playerRefs.weaponCollider.enabled = false; playerRefs.playerVFX.EV_HideTrail(); }
    public void EV_AddForce(float multiplier = 1)
    {
        //Make equivalent between min and max distance to -0,5 and 1 (normalize)
        float equivalent;
        if (distanceToEnemy.Value > maxDistance*multiplier) { equivalent = CalculateEquivalent(defaultDistance.Value); } //If the player is too far, behave with default. Estic multiplicant la max distance pel multiplier no se si va be aixo
        else { equivalent = CalculateEquivalent(distanceToEnemy.Value); } // Else calculate with distance

        
        Vector3 tempForceDirection = playerRefs.followMouse.SwordDirection;
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, tempForceDirection * multiplier, 0.1f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            Force * equivalent,
            addForceTime,
            tempForceDirection,
            attackCurve
            ));
    }
    public void EV_JustAddForce(float multiplier)
    {
        Vector3 forceDirection = playerRefs.followMouse.gameObject.transform.up;
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, forceDirection, 0.1f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            Force * multiplier,
            addForceTime,
            forceDirection,
            attackCurve
            ));
    }
    float CalculateEquivalent(float Distance)
    {
        float inverseF = Mathf.InverseLerp(minDistance, maxDistance, Distance);
        float lerpF = Mathf.Lerp(minForce, maxForce, inverseF);
        return lerpF;
    }
}
 

