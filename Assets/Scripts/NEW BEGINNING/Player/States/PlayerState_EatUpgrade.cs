using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_EatUpgrade : PlayerState
{
    [SerializeField] SpriteRenderer playerRelicRenderer;
    public override void OnEnable()
    {
        playerRelicRenderer.sprite = playerRefs.upgradesManager.pickedUpgrade.iconSprite;
        StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, .2f));

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Stopped);

    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
}
