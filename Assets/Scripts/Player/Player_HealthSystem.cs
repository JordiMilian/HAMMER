using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthSystem : Generic_HealthSystem
{
    public override void Death()
    {
        if (OnDeath != null) OnDeath(this, EventArgs.Empty);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
