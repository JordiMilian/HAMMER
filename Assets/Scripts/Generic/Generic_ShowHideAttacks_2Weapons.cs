using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Generic_ShowHideAttacks_2Weapons : Generic_ShowHideAttackCollider
{
    [SerializeField] Generic_DamageDealer damageDealer_W2;
    public TrailRenderer trailrendered_W2;
    public TrailRenderer testTrailrendered_W2;
    public void EV_Enemy_ShowAttackCollider_W2()
    {
        damageDealer_W2.GetComponent<Collider2D>().enabled = true;
        if (isTesting) { testTrailrendered_W2.emitting = true; return; }
        if (trailrendered_W2 != null) trailrendered_W2.emitting = true;
        eventSystem.OnShowCollider?.Invoke();

    }
    public void EV_Enemy_HideAttackCollider_W2()
    {
        damageDealer_W2.GetComponent<Collider2D>().enabled = false;
        if (isTesting) { testTrailrendered_W2.emitting = false; return; }
        if (trailrendered_W2 != null) trailrendered_W2.emitting = false;
    }
}
