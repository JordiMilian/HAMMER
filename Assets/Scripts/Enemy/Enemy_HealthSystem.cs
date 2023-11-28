using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    [SerializeField] GameObject BloodCristals;
    [SerializeField] int AmountOfCristals;
    void Start() 
    {
        CurrentHealth = MaxHealth;
    }

    
    public void UpdateLife(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            Death();
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    void Death()
    {
        if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
        if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
            
        }

        Destroy(gameObject);
    }

}
