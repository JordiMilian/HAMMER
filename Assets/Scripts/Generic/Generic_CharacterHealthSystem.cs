using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Generic_EventSystem;

public class Generic_CharacterHealthSystem : MonoBehaviour
{
    /* TO DELETE?
    [SerializeField] Generic_References Refs;
    [HideInInspector] public EntityStats baseStats;
    [HideInInspector] public EntityStats currentStats;
    
    [SerializeField] bool FillHealthOnStart = true;

    private void Start()
    {
        if (FillHealthOnStart) { RestoreAllHealth(); }
    }
    private void OnEnable()
    {
        Refs.genericEvents.OnReceiveDamage += (object sender, ReceivedAttackInfo info) => RemoveLife(info.Damage, info.Attacker);
    }
    private void OnDisable()
    {
        Refs.genericEvents.OnReceiveDamage -= (object sender, ReceivedAttackInfo info) => RemoveLife(info.Damage, info.Attacker);
    }
    public void RemoveLife(float damage, GameObject damager)
    {
        if(currentStats.CurrentHp <= 0) { Debug.LogWarning("Tried to kill what is already dead wtf?"); return; }

        currentStats.CurrentHp = (currentStats.CurrentHp - damage);

        if (currentStats.CurrentHp <= 0f)
        {
            Death(damager);
            currentStats.CurrentHp = 0;
        }
        else if (currentStats.CurrentHp > currentStats.MaxHp)
        {
            currentStats.CurrentHp = (currentStats.MaxHp);
        }
    }
    public void RestoreAllHealth()
    {
        currentStats.CurrentHp = (currentStats.MaxHp);
    }
    public virtual void Death(GameObject killer)
    {
        if (Refs.genericEvents.OnDeath != null) Refs.genericEvents.OnDeath(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,killer));
        Destroy(gameObject);
    }
    */
}
