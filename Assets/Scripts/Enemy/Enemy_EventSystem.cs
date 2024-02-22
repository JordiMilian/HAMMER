using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_EventSystem : Generic_EventSystem
{
    public Action OnAgrooPlayer;
    public Action OnPlayerOutOfRange;
    public Action OnStanceBroken;
}
