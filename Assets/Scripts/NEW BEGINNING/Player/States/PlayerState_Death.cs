using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Death : PlayerState
{
    public override void OnEnable()
    {
        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Stopped);

        playerRefs.followMouse.UnfocusCurrentEnemy();

        //Spawn dead heads

        //Wait and show UI

        //Change state to respawning?
        //Or load new scene?
    }
}
