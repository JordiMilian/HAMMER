using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_DamageDetector;

public class Generic_HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField] bool FillHealthOnStart = true;
    [SerializeField] Generic_DamageDetector damageDetector;

    void Start()
    {
        if (FillHealthOnStart) { CurrentHealth = MaxHealth; }
    }
    private void OnEnable()
    {
        damageDetector.OnReceiveDamage += RemoveLife;
    }
    private void OnDisable()
    {
        damageDetector.OnReceiveDamage -= RemoveLife;
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
    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
