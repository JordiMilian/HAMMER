using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_EnteringRoom : PlayerState
{
    [SerializeField] AnimationCurve walkingMovementCurve;
    float averageCurveValue = -1;
    [SerializeField] float distanceToMove;
    [SerializeField] float SecondsToMove;
    Coroutine currentCoroutine;
    public override void OnEnable()
    {
        if(averageCurveValue < 0) { averageCurveValue = UsefullMethods.GetAverageValueOfCurve(walkingMovementCurve, 10); }
        base.OnEnable();
        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Stopped);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Stopped);
        TargetGroupSingleton.Instance.RemoveMouseTarget();
        playerRefs.hideSprites.ShowPlayerSprites();

        playerRefs.transform.position = Vector2.zero;
        animator.CrossFade(AnimatorStateName, transitionTime_instant);
        currentCoroutine = StartCoroutine(EnterRoomCoroutine());

    }
    IEnumerator EnterRoomCoroutine()
    {
        if (averageCurveValue < 0) { averageCurveValue = UsefullMethods.GetAverageValueOfCurve(walkingMovementCurve, 10); }
        yield return StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(playerRefs.characterMover, distanceToMove, SecondsToMove, Vector2.up, walkingMovementCurve));

        stateMachine.ForceChangeState(playerRefs.IdleState);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        TargetGroupSingleton.Instance.ReturnMouseTarget();
    }
}
