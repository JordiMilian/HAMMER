using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01_GreenBoss : Enemy01
{
    [SerializeField] Enemy_AgrooMovement movement01;
    [SerializeField] Enemy_AgrooMovement movement02;
    [SerializeField] GreenBoss_EventSystem GreenEventSystem;

    public override void OnEnable()
    {
        base.OnEnable();
        GreenEventSystem.OnPhase01 += phase01;
        GreenEventSystem.OnPhase02 += phase02;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        GreenEventSystem.OnPhase01 -= phase01;
        GreenEventSystem.OnPhase02 -= phase02;
    }
    void phase01(object sender, EventArgs args)
    {
        enemyMovement = movement01;
    }
    void phase02(object sender, EventArgs args)
    {
        enemyMovement = movement02;
    }

}
