using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_EventSystem : Generic_EventSystem
{
    public Action OnFocused;
    public Action OnUnfocused;
    public Action OnEnterAgroo;
    public Action OnExitAgroo;
    public Action OnPlayerDetected;
    public Action OnStanceBroken;
    public Action OnThrowTomato;
    public Action OnThrowGreenProjectile;
    public Action OnThrowSingleSaw; //In polygon throw it is called each X saws to not overflow the audio mixer
}
