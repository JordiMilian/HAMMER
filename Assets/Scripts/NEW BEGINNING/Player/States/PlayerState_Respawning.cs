using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Respawning : PlayerState
{
    [HideInInspector] Player_Respawner nextRespawner; //Before calling this state, we should fill this respawner 
    public override void OnEnable()
    {
        base.OnEnable();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Stopped);
        //Set up weapon rotation
        Transform weaponPivot = playerRefs.weaponPivot.transform;
        weaponPivot.eulerAngles = new Vector3(
                    weaponPivot.transform.eulerAngles.x,
                    weaponPivot.transform.eulerAngles.y,
                    90
                    );

        //Hide sprites
        //move player to respawner position
        //activate respawner animation
        //respawner animation call EV_ from here
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCorotine != null) { StopCoroutine(currentCorotine); }
    }
    Coroutine currentCorotine;
    public void EV_ActuallyRespawn()
    {
        currentCorotine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_instant));
        CameraShake.Instance.ShakeCamera(0.5f, 0.3f);
        //Show sprites

    }
}
