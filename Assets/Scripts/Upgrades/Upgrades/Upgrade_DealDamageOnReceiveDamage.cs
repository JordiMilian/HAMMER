using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/DealDamageOnAttacked", fileName = "DealDamageOnAttacked")]
public class Upgrade_DealDamageOnReceiveDamage : Upgrade
{
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();
        playerRefs.GetComponent<IDamageReceiver>().OnDamageReceived_event += onDamageReceived;
    }

    private void onDamageReceived(ReceivedAttackInfo info)
    {
        if (info.AttackerRoot_Go.TryGetComponent<IDamageReceiver>(out var receiver))
        {
          receiver.OnDamageReceived(
          new ReceivedAttackInfo(
              info.CollisionPosition,
          -info.RootsDirection,
          -info.CollidersDirection,
          playerRefs.gameObject,
          playerRefs.DamageDealersList[0],
          .5f * playerRefs.currentStats.DamageMultiplicator,
          .25f,
          false,
          .5f
          ));
        }    }


    public override void onRemoved(GameObject entity)
    {
        playerRefs.GetComponent<IDamageReceiver>().OnDamageReceived_event -= onDamageReceived;
    }

    public override string shortDescription()
    {
        return "Deal back damage to enemy when attacked";
    }
}
