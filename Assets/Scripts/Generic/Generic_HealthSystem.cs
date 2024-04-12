using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Generic_EventSystem;

public class Generic_HealthSystem : MonoBehaviour
{
    //public float MaxHealth;
    //public float CurrentHealth;
    public FloatReference MaxHP;
    public FloatReference CurrentHP;
    [SerializeField] bool FillHealthOnStart = true;
    [SerializeField] Generic_Stats stats;
    public Generic_EventSystem eventSystem;

    void Start()
    {
        MaxHP.Value = stats.MaxHealth;
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
    public void RemoveLife(object sender, ReceivedAttackInfo receivedAttackInfo)
    {
        CurrentHP.Value -= receivedAttackInfo.Damage;

        if (CurrentHP.Value <= 0)
        {
            Death(receivedAttackInfo.Attacker);
        }
        if (CurrentHP.Value > MaxHP.Value)
        {
            CurrentHP.Value = MaxHP.Value;
        }
        if (eventSystem.OnUpdatedHealth != null) eventSystem.OnUpdatedHealth();
    }
    public void RestoreAllHealth()
    {
        CurrentHP.Value = MaxHP.Value;
        eventSystem.OnUpdatedHealth?.Invoke();
    }
    public virtual void Death(GameObject killer)
    {
        if (eventSystem.OnDeath != null) eventSystem.OnDeath(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,killer));
        Destroy(gameObject);
    }
}
