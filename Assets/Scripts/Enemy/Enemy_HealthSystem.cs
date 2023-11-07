using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField] GameObject deadBody;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    
    public void UpdateLife(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            var DeadBody = Instantiate(deadBody, transform.position,Quaternion.Euler(0,0,0));
            Destroy(gameObject);
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

}
