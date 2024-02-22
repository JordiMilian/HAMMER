using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Generic_EventSystem;

public class Generic_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField] bool FillHealthOnStart = true;
    [SerializeField] Generic_Stats stats;
    public Generic_EventSystem eventSystem;

    void Start()
    {
        MaxHealth = stats.MaxHealth;
        if (FillHealthOnStart) { RestoreAllHealth(); }
    }
    private void OnEnable()
    {
        eventSystem.OnReceiveDamage += RemoveLife;
    }
    private void OnDisable()
    {
        eventSystem.OnReceiveDamage -= RemoveLife;
    }
    public void RemoveLife(object sender, EventArgs_ReceivedAttackInfo receivedAttackInfo)
    {
        CurrentHealth -= receivedAttackInfo.Damage;

        if (CurrentHealth <= 0)
        {
            Death();
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        if (eventSystem.OnUpdatedHealth != null) eventSystem.OnUpdatedHealth();
    }
    public void RestoreAllHealth()
    {
        CurrentHealth = MaxHealth;
    }
    public virtual void Death()
    {
        if (eventSystem.OnDeath != null) eventSystem.OnDeath();
        Destroy(gameObject);
    }
}
