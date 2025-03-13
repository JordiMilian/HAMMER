using System.Collections;
using System.Collections.Generic;
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
}
