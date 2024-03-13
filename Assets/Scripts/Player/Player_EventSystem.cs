using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    
    public Action OnPerformRoll;
    public EventHandler OnRespawn;
    public Action OnPerformAttack;
}
