using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_EnteringRoom : PlayerState
{
    [SerializeField] AnimationCurve walkingMovementCurve;
    float averageCurveValue = -1;
    [SerializeField] float distanceToMove;
    [SerializeField] float SecondsToMove;
    public override void OnEnable()
    {
        if(averageCurveValue < 0) { averageCurveValue = UsefullMethods.GetAverageValueOfCurve(walkingMovementCurve, 10); }
        base.OnEnable();
        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Stopped);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Stopped);

        //move player to 0,0
        //push them forward
        if (averageCurveValue < 0) { averageCurveValue = UsefullMethods.GetAverageValueOfCurve(walkingMovementCurve, 10); }
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(playerRefs.characterMover, distanceToMove, SecondsToMove, Vector2.up, walkingMovementCurve));
        //play walking animation
        //return input
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
}
