using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWeaponEnemy_AnimationEvents : BasicEnemy_AnimationEvents
{
    [SerializeField] Collider2D Weapon2_Collider;
    [SerializeField] TrailRenderer Weapon2_TrailRenderer;
    public void EV_Enemy_ShowAttackCollider_W2()
    {
        Weapon2_Collider.enabled = true;
        Weapon2_Collider.GetComponent<Generic_DamageDealer>().ResetDetectedReceivers();

        if (Weapon2_TrailRenderer != null) { Weapon2_TrailRenderer.emitting = true; }
        SFX_PlayerSingleton.Instance.playSFX(SFX_Swing, 0.2f);
    }
    public void EV_Enemy_HideAttackCollider_W2()
    {
        Weapon2_Collider.enabled = false;

        if (Weapon2_TrailRenderer != null) { Weapon2_TrailRenderer.emitting = false; }
        SFX_PlayerSingleton.Instance.playSFX(SFX_Swing, 0.2f);
    }
}
