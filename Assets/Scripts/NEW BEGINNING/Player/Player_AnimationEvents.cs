using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_AnimationEvents : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    #region DAMAGE COLLIDERS
    [SerializeField] AudioClip SFX_SwordSwing;
    public void EV_Enemy_ShowAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.GetComponent<Collider2D>().enabled = true;
        }
        playerRefs.weaponTrail.emitting = true;
        SFX_PlayerSingleton.Instance.playSFX(SFX_SwordSwing,0.1f);
    }
    public void EV_Enemy_HideAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.GetComponent<Collider2D>().enabled = false;
        }
        playerRefs.weaponTrail.emitting = false;
    }
    #endregion
    #region PARRY COLLIDERS
    public void EV_ShowParryCollider()
    {
        playerRefs.parryCollider.enabled = true;
        playerRefs.damageDetectorCollider.enabled = false;
    }
    public void EV_HideParryColldier()
    {
        playerRefs.parryCollider.enabled = false;
        playerRefs.damageDetectorCollider.enabled = true;
    }
    #endregion
    #region HIDE COLLIDER FOR ROLL
    public void EV_HidePlayerCollider()
    {
        gameObject.layer = 15;
        playerRefs.damageDetectorCollider.enabled = false;
        playerRefs.characterMover.ignoreRay = true;
    }
    public void EV_ShowPlayerCollider()
    {
        gameObject.layer = 20;
        playerRefs.damageDetectorCollider.enabled = true;
        playerRefs.characterMover.ignoreRay = false;
    }
    #endregion
    #region STATE MACHINE CAN TRANSITION AND INPUT 
    public void EV_CanTransition()
    {
        playerRefs.stateMachine.EV_CanTransition();
    }
    public void EV_ReturnInput()
    {
        playerRefs.stateMachine.EV_ReturnInput();
    }
    #endregion

    public void EV_SetMovementSpeed(MovementSpeeds speedType)
    {
        playerRefs.movement2.SetMovementSpeed(speedType);
    }

    [SerializeField] PlayerState_SpecialHeal specialHealState;
    public void EV_ActuallyHeal()
    {
        specialHealState.ActuallyHeal();
        
    }
    #region ADD FORCES
    [Header("Add Force Stats")]
    [SerializeField] float minDistance, maxDistance;
    [SerializeField] float minForce, maxForce;
    [SerializeField] float defaulDistance;
    [SerializeField] float addForceTime;
    [SerializeField] AnimationCurve attackCurve;
    [SerializeField] float forceMultiplier;

    public void EV_AddForce(float multiplier = 1)
    {
        //If(followMOuse.ItsFocusing){getDistanceToEnemy(followMouse.focusedEneny)
        //else {proximityDetector.getAttackDistance()}

        float AttackDistance = playerRefs.proximityDetector.GetAttackDistance();
        //Make equivalent between min and max distance to -0,5 and 1 (normalize)
        float equivalent;
        if (AttackDistance > maxDistance * multiplier) { equivalent = CalculateEquivalent(defaulDistance); } //If the player is too far, behave with default. Estic multiplicant la max distance pel multiplier no se si va be aixo
        else { equivalent = CalculateEquivalent(AttackDistance); } // Else calculate with distance


        Vector3 tempForceDirection = playerRefs.followMouse.SwordDirection;
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, tempForceDirection * multiplier, 0.1f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            forceMultiplier * equivalent,
            addForceTime,
            tempForceDirection,
            attackCurve
            ));
    }
    float CalculateEquivalent(float distance)
    {
        float inverseF = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float lerpF = Mathf.Lerp(minForce, maxForce, inverseF);
        return lerpF;
    }
    public void EV_JustAddForce(float multiplier)
    {
        Vector3 forceDirection = playerRefs.followMouse.gameObject.transform.up;
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, forceDirection, 0.1f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            forceMultiplier * multiplier,
            addForceTime,
            forceDirection,
            attackCurve
            ));
    }
    #endregion
}
