using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    public static Action<int> OnBeatBoss;
    public static Action OnPlayerDeath;
    public static Action OnPlayerReappear;
}
