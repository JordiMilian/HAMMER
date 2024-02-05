using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ShowHideAttackCollider : MonoBehaviour
{
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] TrailRenderer trailrendered;
    public void EV_Enemy_ShowAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = true;
        if (trailrendered != null) trailrendered.emitting = true;

    }
    public void EV_Enemy_HideAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = false;
        if (trailrendered != null) trailrendered.emitting = false;
    }
}
