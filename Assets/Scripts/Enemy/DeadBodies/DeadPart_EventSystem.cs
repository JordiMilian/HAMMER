using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_EventSystem : Generic_EventSystem
{

    //public EventHandler<DeadPartArgs> OnBeingAttacked;
    //public EventHandler<ObjectDirectionArgs> OnBeingTouched;
    public Action OnHitWall;
}
