using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_EventSystem : MonoBehaviour
{

    //public EventHandler<DeadPartArgs> OnBeingAttacked;
    //public EventHandler<ObjectDirectionArgs> OnBeingTouched;
    public Action OnHitWall;
}
