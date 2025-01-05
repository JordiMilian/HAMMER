using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ShowHideCollider : Base_ShowHideAttackCollider
{
    [SerializeField] List<Generic_DamageDealer> damageDealer = new List<Generic_DamageDealer>();

    [SerializeField] Player_References playerRefs;

    [SerializeField] TrailRenderer trailrendered;
    private void OnEnable()
    {
        playerRefs.events.OnEnterIdle += EV_Enemy_HideAttackCollider;
    }
    private void OnDisable()
    {
        playerRefs.events.OnEnterIdle -= EV_Enemy_HideAttackCollider;
    }
    public override void EV_Enemy_ShowAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in damageDealer)
        {
            dealer.GetComponent<Collider2D>().enabled = true;
        }
        if (trailrendered != null) trailrendered.emitting = true;
    }
    public override void EV_Enemy_HideAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in damageDealer)
        {
            dealer.GetComponent<Collider2D>().enabled = false;
        }
        if (trailrendered != null) trailrendered.emitting = false;
    }
}
