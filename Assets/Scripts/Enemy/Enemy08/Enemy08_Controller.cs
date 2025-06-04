using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy08_Controller : BasicEnemy_Controller
{
    private void Start()
    {
        enemyRefs.basicAnimationEvents.EV_Enemy_HideAttackCollider();
        enemyRefs.GetComponent<TwoWeaponEnemy_AnimationEvents>().EV_Enemy_HideAttackCollider_W2();
    }

}
