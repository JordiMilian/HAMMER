using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    public class EventArgs_StaminaConsumption
    {
        public float StaminaUsage;
        public EventArgs_StaminaConsumption(float stamina)
        {
            StaminaUsage = stamina;
        }
    }
    public Action OnPerformRoll;
    public EventHandler OnRespawn;
    public Action OnPerformAttack;
    public EventHandler<EventArgs_StaminaConsumption> OnStaminaAction;
}
