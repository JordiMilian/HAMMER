using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthSystem : Generic_HealthSystem
{
    public override void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
