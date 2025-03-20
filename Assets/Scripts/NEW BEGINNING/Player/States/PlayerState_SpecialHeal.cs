using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerState_SpecialHeal : PlayerState
{
    [SerializeField] AudioClip SFX_Heal;
    Coroutine currentCoroutine;
    float amountToHeal;
    public override void OnEnable()
    {
        base.OnEnable();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Slow);
        amountToHeal = playerRefs.currentStats.MaxHp - playerRefs.currentStats.CurrentHp;
        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_long));
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
    }

    public void ActuallyHeal()
    {
        playerRefs.currentStats.CurrentBloodFlow = 0;

        playerRefs.GetComponent<IHealth>().RemoveHealth(-amountToHeal);

        SFX_PlayerSingleton.Instance.playSFX(SFX_Heal);

        playerRefs.flasher.CallDefaultFlasher();

        CameraShake.Instance.ShakeCamera(.2f, 0.1f);

        
    }
}
