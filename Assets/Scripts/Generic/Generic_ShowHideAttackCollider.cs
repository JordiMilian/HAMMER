using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ShowHideAttackCollider : MonoBehaviour
{
    [SerializeField] Generic_DamageDealer damageDealer;
    public TrailRenderer trailrendered;
    public TrailRenderer testTrailrendered;
    [HideInInspector] public bool isTesting;
    public void EV_Enemy_ShowAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = true;
        if (isTesting) { testTrailrendered.emitting = true; return; }
        if (trailrendered != null) trailrendered.emitting = true;

    }
    public void EV_Enemy_HideAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = false;
        if (isTesting) { testTrailrendered.emitting = false; return; }
        if (trailrendered != null) trailrendered.emitting = false;
    }
}
