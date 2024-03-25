using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_EventSystem : Generic_EventSystem
{
    public class DeadPartArgs : EventArgs
    {
        public Vector2 Direction;
        public DeadPartArgs(Vector2 direction)
        {
            Direction = direction;
        }
    }
    public EventHandler<ObjectDirectionArgs> OnSpawned;
    //public EventHandler<DeadPartArgs> OnBeingAttacked;
    //public EventHandler<ObjectDirectionArgs> OnBeingTouched;
    public Action OnHitWall;
}
