using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_ExtraComboAttack : PlayerState_BasicComboAttack
{
    [SerializeField] AudioClip SFX_ExtraComboAttack;
    [SerializeField] Gradient weaponTrailGradient;
    public override void OnEnable()
    {
        base.OnEnable();
        playerRefs.weaponTrail.colorGradient = weaponTrailGradient;
        playerRefs.animationEvents.OnShowAttackCollider += OnShowAttackCollider;
    }
    void OnShowAttackCollider() { SFX_PlayerSingleton.Instance.playSFX(SFX_ExtraComboAttack); }
    public override void OnDisable()
    {
        base.OnDisable();
        playerRefs.weaponTrail.colorGradient = playerRefs.baseWeaponGradient;
        playerRefs.animationEvents.OnShowAttackCollider -= OnShowAttackCollider;
    }
}
