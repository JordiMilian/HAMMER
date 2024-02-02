using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_EventsSystem : MonoBehaviour
{
    public class EventArgs_DealtDamageInfo
    {
        public Vector3 CollisionPosition;
        public EventArgs_DealtDamageInfo(Vector3 collisionPosition)
        {
            CollisionPosition = collisionPosition;
        }
    }
}
