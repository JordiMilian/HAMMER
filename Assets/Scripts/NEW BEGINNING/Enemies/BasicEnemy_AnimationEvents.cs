using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy_AnimationEvents : MonoBehaviour
{
    //set rotation speed
    //set movement speed
    //hide and show weapon colliders
    [SerializeField] protected Enemy_References enemyRefs;
    [SerializeField] TrailRenderer weaponTrail;
    [SerializeField] protected AudioClip SFX_Swing;

    SpeedsEnum StringToSpeedEnum(string speed)
    {
        switch (speed)
        {
            case "Regular":
                return SpeedsEnum.Regular;
            case "Fast":
                return SpeedsEnum.Fast;
            case "Slow":
                return SpeedsEnum.Slow;
            case "VerySlow":
                return SpeedsEnum.VerySlow;
            case "Stopped":
                return SpeedsEnum.Stopped;
            default:
                Debug.LogError("Invalid speed string: " + speed);
                return SpeedsEnum.Regular; 
        }
    }

   public void EV_SetMovementSpeed(string speed)
   {
       enemyRefs.moveToTarget.SetMovementSpeed(StringToSpeedEnum(speed));
   }
   public void EV_SetRotationSpeed(string speed)
   {
       enemyRefs.moveToTarget.SetRotatinSpeed(StringToSpeedEnum(speed));
   }   
    public void EV_SetMovementAndRotationSpeed(string speed)
    {
        enemyRefs.moveToTarget.SetMovementSpeed(StringToSpeedEnum(speed));
        enemyRefs.moveToTarget.SetRotatinSpeed(StringToSpeedEnum(speed));
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
