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
    [SerializeField] Generic_References Refs;
    public FloatReference MaxHP;
    public FloatReference CurrentHP;
    [SerializeField] bool FillHealthOnStart = true;

    void Start()
    {
        MaxHP.Value = Refs.stats.MaxHealth;
        if (FillHealthOnStart) { RestoreAllHealth(); }
    }
    private void OnEnable()
    {
        Refs.genericEvents.OnReceiveDamage += RemoveLife;
    }
    private void OnDisable()
    {
        Refs.genericEvents.OnReceiveDamage -= RemoveLife;
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
        if (Refs.genericEvents.OnUpdatedHealth != null) Refs.genericEvents.OnUpdatedHealth();
    }
    public void RestoreAllHealth()
    {
        CurrentHP.Value = MaxHP.Value;
        Refs.genericEvents.OnUpdatedHealth?.Invoke();
    }
    public virtual void Death(GameObject killer)
    {
        if (Refs.genericEvents.OnDeath != null) Refs.genericEvents.OnDeath(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,killer));
        Destroy(gameObject);
    }
}
