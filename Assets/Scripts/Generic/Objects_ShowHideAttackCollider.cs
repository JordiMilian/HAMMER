using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects_ShowHideAttackCollider : Base_ShowHideAttackCollider
{
    [SerializeField] Collider2D DamageCollider;

    

    public override void EV_Enemy_ShowAttackCollider()
    {
        DamageCollider.enabled = true;
    }
    public override void EV_Enemy_HideAttackCollider()
    {
        DamageCollider.enabled = false;
    }
 
}
