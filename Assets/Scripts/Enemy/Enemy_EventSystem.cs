using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_EventSystem : Generic_EventSystem
{
    public Action OnFocused;
    public Action OnUnfocused;
    public Action CallAgrooState;
    public Action CallIdleState;
    public Action OnAgrooState;
    public Action OnIdleState;
    public Action OnStanceBroken;
}
