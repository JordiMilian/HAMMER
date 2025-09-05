using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_ThrowFrisbeeAttack : EnemyState_Attack
{
    //The frisbee should have a character mover?
    public override void OnEnable()
    {
        base.OnEnable();
        //Play throw frisbee animation
        //transition to Idle when its over
    }
    public override void Update()
    {
        //after throwing, follor the frisbee around. 
        //If frisbee is near enough, change catch it
    }
    public void OnThrowFrisbee()
    {
        //Called from the Frisbee thrower script
        //instantiate the frisbee towards the player
        //Wait the throw frisbee time(found in FrisbeeController)
        //
        //
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }
}
