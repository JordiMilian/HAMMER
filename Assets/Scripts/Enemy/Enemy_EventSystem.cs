using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_EventSystem : Generic_EventSystem
{
    public EventHandler OnAgrooPlayer;
    public EventHandler OnPlayerOutOfRange;
    public EventHandler OnStanceBroken;
}
