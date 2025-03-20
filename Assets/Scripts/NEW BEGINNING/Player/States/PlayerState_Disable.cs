using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Disable : PlayerState
{
    //Finish whatever is doing?
    //Dont subscribte
    //movement to zero
    //idle animation
    public override void OnEnable()
    {
        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Stopped);
    }

}
