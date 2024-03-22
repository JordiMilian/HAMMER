using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_EventSystem_min : MonoBehaviour
{
    public class DeadPartArgs : EventArgs
    {
        public Vector2 Direction;
        public DeadPartArgs(Vector2 direction)
        {
            Direction = direction;
        }
    }
    public EventHandler<DeadPartArgs> OnSpawned;
    public EventHandler<DeadPartArgs> OnBeingAttacked;
    public EventHandler<DeadPartArgs> OnBeingTouched;
    public Action OnHitWall;
}
