using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : ScriptableObject
{

    [SerializeField] private float _maxHp;
    [SerializeField] private float _speed;
    [SerializeField] private float _damageMultiplicator;

    // Eventos
    [HideInInspector] public Action<float> OnMaxHpChange;
    [HideInInspector] public Action<float> OnSpeedChange;
    [HideInInspector] public Action<float> OnDamageMultiplicatorChange;

    public float MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            OnMaxHpChange?.Invoke(_maxHp);
        }
    }

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            OnSpeedChange?.Invoke(_speed);
        }
    }

    public float DamageMultiplicator
    {
        get => _damageMultiplicator;
        set
        {
            _damageMultiplicator = value;
            OnDamageMultiplicatorChange?.Invoke(_damageMultiplicator);
        }
    }
}
