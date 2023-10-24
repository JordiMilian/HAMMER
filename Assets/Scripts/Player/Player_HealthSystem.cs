using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }


    public void UpdateLife(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (CurrentHealth > MaxHealth) { CurrentHealth = MaxHealth; }
    }
}
