using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthSystem : MonoBehaviour
{
    ReloadSceneOnDeath reloadScene;
    public float MaxHealth;
    public float CurrentHealth;
    void Start()
    {
        reloadScene = GetComponent<ReloadSceneOnDeath>();
        CurrentHealth = MaxHealth;
    }


    public void UpdateLife(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            reloadScene.ReloadScene();
        }
        if (CurrentHealth > MaxHealth) { CurrentHealth = MaxHealth; }
    }
}
