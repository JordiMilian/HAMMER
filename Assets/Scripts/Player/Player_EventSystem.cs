using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    public class EventArgs_ParryInfo : EventArgs
    {
        public Vector3 vector3data;
        public EventArgs_ParryInfo(Vector3 data)
        {
            vector3data = data;
        }
    }
    public EventHandler OnPerformRoll;
    public EventHandler<EventArgs_ParryInfo> OnSuccessfulParry;
    public EventHandler OnRespawn;
}
