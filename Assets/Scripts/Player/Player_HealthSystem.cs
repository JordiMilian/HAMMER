using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthSystem : Generic_HealthSystem
{
    [SerializeField] Player_References playerRefs;

    public override void Death(GameObject killer)
    {
        playerRefs.events.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(gameObject, killer));
        GameEvents.OnPlayerDeath?.Invoke();

        TimeScaleEditor.Instance.SlowMotion(80, 2);
    }

}
