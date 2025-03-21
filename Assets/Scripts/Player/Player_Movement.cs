using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    PlayerStats currentStats;

    [Header("BASE MOVEMENT")]
    //public float CurrentSpeed;
    //public float BaseSpeed;
    //public float velocityMultiplier = 1;

    [Header("RUN")]
    [SerializeField] float RunningSpeed = 40;
    [SerializeField] float RunInputDelayTime = 0.5f;
    [SerializeField] float StaminaPerSecond = 1;
    float TimerDelay;
    bool IsWaitingInputDelay;
    bool isRunning = false;


    [Header("ROLL")]
    [SerializeField] float RollTime;
    [SerializeField] float RollDistance;
    [SerializeField] bool updateAverageSize_trigger;
    float rollCurve_averageValue = 0;
    public AnimationCurve RollCurve;


    private void OnEnable()
    {
        Physics.IgnoreLayerCollision(15, 16);
    }

    /* KILL
    void OnRollUnpressed()
    {
        if (playerRefs.currentStats.CurrentStamina <= 0) { return; }


        if (IsWaitingInputDelay)
        {
            IsWaitingInputDelay = false;

            //Multiply the looking direction with the Input direction:
            //If they coincide, the direction will be 1 and player is looking forward, else its -1 and its looking backwards
            //
            //Esto es una cutrada ficarho aqui pero weno funcione
            int direction = UsefullMethods.normalizeFloat(Input.GetAxisRaw("Horizontal")) * playerRefs.spriteFliper.lookingDirection;
            playerRefs.animator.SetInteger("LookingDirection", direction);

            playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Roll"));
        }
    }
   
    void Move(Vector2 vector2)
    {
        playerRefs.characterMover.MovementVectorsPerSecond.Add(vector2.normalized * currentStats.Speed);
        WalkingAnimation();
    }
  
    void WalkingAnimation()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
        {
            playerRefs.animator.SetBool("Walking", true);
        }
        else
        {
            playerRefs.animator.SetBool("Walking", false);
        }
    }
    void CallDashMovement()
    {
        //Call the event to remove Stamina
        //playerRefs.events.CallStaminaAction?.Invoke(1.25f);

        //Find the direction. If there is no direction, return???? maybe nose
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll

        //If the player is not imputing a direction, rotate to the oposite of the sword
        if (Axis.magnitude == 0) 
        {
            Vector2 opositeDirectionToSword = -playerRefs.followMouse.SwordDirection;
            StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
             playerRefs.characterMover,
             RollDistance,
             RollTime,
             opositeDirectionToSword,
             RollCurve,
             rollCurve_averageValue
             ));
            return;
        }

        //Else roll towards imput direction
        //StartCoroutine(DashMovement(Axis));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            RollDistance,
            RollTime,
            Axis,
            RollCurve,
            rollCurve_averageValue
            ));
    }
    
    */
  
}
