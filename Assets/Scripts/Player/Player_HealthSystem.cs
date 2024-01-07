using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthSystem : Generic_HealthSystem
{
    ReloadSceneOnDeath reloadScene;
   
    void Start()
    {
        reloadScene = GetComponent<ReloadSceneOnDeath>();
    }

    public override void Death()
    {
        reloadScene.ReloadScene();
    }

}
