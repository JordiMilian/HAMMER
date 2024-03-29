using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthSystem : Generic_HealthSystem
{
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    public FloatVariable currentHP;

    private void Awake()
    {
    }
    public override void Death(GameObject killer)
    {
        eventSystem.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(gameObject, killer));
        /*
        if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
        if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        */
        TimeScaleEditor.Instance.SlowMotion(80, 2);
    }

}
