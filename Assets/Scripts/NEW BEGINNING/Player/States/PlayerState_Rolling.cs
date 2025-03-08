using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Rolling : PlayerState
{
    Player_References playerRefs;

    [SerializeField] float RollTime; //This should depend on the animation I think?
    [SerializeField] float RollDistance;
    [SerializeField] bool updateAverageSize_trigger;
    float rollCurve_averageValue = 0;

    [SerializeField] AnimationCurve RollCurve;
    Coroutine rollCoroutine;
    public override void OnEnable()
    {
        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll

        //Stop regular movement during roll? Or slow it dow

        //Handle which direction it's facing
        
        PerformRoll();

    }
    void PerformRoll()
    {
        //Maybe InputDetector should be involced in this??
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        Vector2 directionToRoll = Axis;

        //If the player is not imputing a direction, rotate to the oposite of the sword
        if (Axis.magnitude < 0.1f)
        {
            Vector2 opositeDirectionToSword = -playerRefs.followMouse.SwordDirection;
            directionToRoll = opositeDirectionToSword;
        }

        rollCoroutine = StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            RollDistance,
            RollTime,
            directionToRoll,
            RollCurve,
            rollCurve_averageValue
            ));
    }
    public override void OnDisable()
    {
        if(rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
        }
    }

}
