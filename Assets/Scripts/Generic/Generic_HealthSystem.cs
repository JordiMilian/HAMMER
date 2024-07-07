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
    [SerializeField] bool isPlayers;

    void Start()
    {
        if (!isPlayers) { MaxHP.ChangeValue(Refs.stats.MaxHealth); }
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
        CurrentHP.ChangeValue(CurrentHP.GetValue() - receivedAttackInfo.Damage);

        if (CurrentHP.GetValue() <= 0f)
        {
            Death(receivedAttackInfo.Attacker);
            CurrentHP.ChangeValue(0);
        }
        if (CurrentHP.GetValue() > MaxHP.GetValue())
        {
            CurrentHP.ChangeValue(MaxHP.Variable.Value);
        }
    }
    public void RestoreAllHealth()
    {
        CurrentHP.ChangeValue(MaxHP.GetValue());
    }
    public virtual void Death(GameObject killer)
    {
        if (Refs.genericEvents.OnDeath != null) Refs.genericEvents.OnDeath(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,killer));
        Destroy(gameObject);
    }
}
