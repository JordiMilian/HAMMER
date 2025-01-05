using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Generic_ShowHideAttacks_2Weapons : Enemy_ShowHideAttackCollider
{
    [SerializeField] Generic_DamageDealer damageDealer_W2;
    public TrailRenderer trailrendered_W2;
    public TrailRenderer testTrailrendered_W2;
    private void OnEnable()
    {
        EV_Enemy_HideAttackCollider();
        EV_Enemy_HideAttackCollider_W2();
        enemyRefs.enemyEvents.OnEnterAgroo += EV_Enemy_HideAttackCollider;
        enemyRefs.enemyEvents.OnEnterAgroo += EV_Enemy_HideAttackCollider_W2;
    }
    private void OnDisable()
    {
        enemyRefs.enemyEvents.OnEnterAgroo -= EV_Enemy_HideAttackCollider;
        enemyRefs.enemyEvents.OnEnterAgroo -= EV_Enemy_HideAttackCollider_W2;
    }
    public override void HideCollliderOnParry()
    {
        base.HideCollliderOnParry();
        EV_Enemy_HideAttackCollider_W2();
    }
    public void EV_Enemy_ShowAttackCollider_W2()
    {
        damageDealer_W2.GetComponent<Collider2D>().enabled = true;
        if (isTesting) { testTrailrendered_W2.emitting = true; return; }
        if (trailrendered_W2 != null) trailrendered_W2.emitting = true;
        enemyRefs.enemyEvents.OnShowCollider?.Invoke();
    }
    public void EV_Enemy_HideAttackCollider_W2()
    {
        damageDealer_W2.GetComponent<Collider2D>().enabled = false;
        if (isTesting) { testTrailrendered_W2.emitting = false; return; }
        if (trailrendered_W2 != null) trailrendered_W2.emitting = false;
    }
}
