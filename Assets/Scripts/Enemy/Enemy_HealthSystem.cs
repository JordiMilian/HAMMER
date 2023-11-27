using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    
    public void UpdateLife(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            if(deadBody != null && deadHead != null)
            {
                var DeadBody = Instantiate(deadBody, transform.position, Quaternion.Euler(0, 0, 0));
                var DeadHead = Instantiate(deadHead, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
            
        
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

}
