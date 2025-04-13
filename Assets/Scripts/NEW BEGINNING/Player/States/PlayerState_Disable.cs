using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Disable : PlayerState
{

    public override void OnEnable()
    {
        playerRefs.movement2.SetMovementSpeed(SpeedsEnum.Stopped);
        //Set rotation speed to zero too
        TargetGroupSingleton.Instance.RemoveMouseTarget();
        playerRefs.animator.CrossFade(AnimatorStateName, transitionTime_instant);
        playerRefs.spriteFliper.canFlip = false;
    }
    public override void OnDisable()
    {
        playerRefs.spriteFliper.canFlip = true;
        TargetGroupSingleton.Instance.ReturnMouseTarget();
    }

}
