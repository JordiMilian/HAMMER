using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy_AnimationEvents : MonoBehaviour
{
    //set rotation speed
    //set movement speed
    //hide and show weapon colliders
    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] TrailRenderer weaponTrail;
    [SerializeField] AudioClip SFX_Swing;
   public void EV_SetMovementSpeed(SpeedsEnum speed)
   {
       enemyRefs.moveToTarget.SetMovementSpeed(speed);
   }
   public void EV_SetRotationSpeed(SpeedsEnum speed)
   {
       enemyRefs.moveToTarget.SetRotatinSpeed(speed);
   }   
    public void EV_SetMovementAndRotationSpeed(SpeedsEnum speed)
    {
        enemyRefs.moveToTarget.SetMovementSpeed(speed);
        enemyRefs.moveToTarget.SetRotatinSpeed(speed);
    }
   public void EV_Enemy_ShowAttackCollider()
    {
        foreach(Generic_DamageDealer dealer in enemyRefs.DamageDealersList)
        {
            dealer.GetComponent<Collider2D>().enabled = true;
        }
        if (weaponTrail != null) { weaponTrail.emitting = true; }
        SFX_PlayerSingleton.Instance.playSFX(SFX_Swing, 0.2f);
    }
    public void EV_Enemy_HideAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in enemyRefs.DamageDealersList)
        {
            dealer.GetComponent<Collider2D>().enabled = false;
        }
        if (weaponTrail != null) { weaponTrail.emitting = false; }
    }
    IAttackedWhileRecovery enemyController;
    public void EV_StartRecovery()
    {
        if(enemyController == null) { enemyController = GetComponent<IAttackedWhileRecovery>(); }

        enemyController.isInRecovery = true;
    }
}
