using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    public Action OnPerformRoll;
    public Action CallRespawn;
    public Action CallHideAndDisable;
    public Action CallDisable;
    public Action CallEnable;
    public Action OnPerformAttack;
    public Action OnPerformParry;
    public Action<GameObject> OnFocusEnemy;
    public Action OnUnfocusEnemy;
    public Action OnPerformSpecialAttack;
    public Action<float> OnStaminaAction;
}
