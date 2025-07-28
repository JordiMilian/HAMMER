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
            dealer.ResetDetectedReceivers();
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
        playerRefs.characterMover.collisionLayer = CollisionLayers.JumpingPlayer;
        playerRefs.damageDetectorCollider.enabled = false;
        
    }
    public void EV_ShowPlayerCollider()
    {
        playerRefs.characterMover.collisionLayer = CollisionLayers.AllCollision;
        playerRefs.damageDetectorCollider.enabled = true;
       
    }
    #endregion
    #region STATE MACHINE CAN TRANSITION AND INPUT 
    public void EV_canTransition()
    {
        playerRefs.stateMachine.EV_CanTransition();
    }
    public void EV_returnInput()
    {
        playerRefs.stateMachine.EV_ReturnInput();
    }
    #endregion

    public void EV_SetMovementSpeed(SpeedsEnum speedType)
    {
        playerRefs.movement.SetMovementSpeed(speedType);
    }
    public void EV_ActuallyHeal()
    {
        PlayerState_SpecialHeal specialHeal = (PlayerState_SpecialHeal)playerRefs.SpecialHealState;
        specialHeal.ActuallyHeal();
        
    }
    public void EV_ActuallyGetUpgrade()
    {
        Player_UpgradesManager upgradesManger = playerRefs.upgradesManager;
        upgradesManger.EV_OnEatUpgrade();
    }
    #region ADD FORCES
    [Header("Add Force Stats")]
    [SerializeField] float addForceTime;
    [SerializeField] AnimationCurve attackCurve;

    public void EV_AddForce()
    {
        float thisDistance = playerRefs.swordRotation.GetAddForceDistance();
        /*
        //Make equivalent between min and max distance to -0,5 and 1 (normalize)
        float equivalent;
        if (thisDistance > maxDistance * multiplier) { equivalent = CalculateEquivalent(defaulDistance); } //If the player is too far, behave with default. Estic multiplicant la max distance pel multiplier no se si va be aixo
        else { equivalent = CalculateEquivalent(thisDistance); } // Else calculate with distance
        */
        Vector3 lookingDirection = (playerRefs.swordRotation.LookingPosition - (Vector2)transform.position).normalized;

        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            thisDistance,
            addForceTime,
            lookingDirection,
            attackCurve
            ));
    }
    public void EV_JustAddForce(float distance)
    {
        Vector3 forceDirection = playerRefs.swordRotation.LookingPosition;
        //StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, forceDirection, 0.1f));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            distance,
            addForceTime,
            forceDirection,
            attackCurve
            ));
    }
    #endregion
    

}
