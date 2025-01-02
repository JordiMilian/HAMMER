using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Entity Stats", menuName = "Entity Stats/EnemyStats")]
public class EnemyStats : EntityStats
{
    [SerializeField] private int _xpDrop;
    [SerializeField] private float _rotationSpeed;

    [HideInInspector] public UnityEvent<float> OnXpDropChange;
    [HideInInspector] public Action<float> OnRotationSpeedChange;

    public int XpToDrop
    {
        get => _xpDrop;
        set
        {
            if (Mathf.Approximately(_xpDrop, value)) return;
            _xpDrop = value;
            OnXpDropChange?.Invoke(_xpDrop);
        }
    }

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set
        {
            _rotationSpeed = value;
            OnRotationSpeedChange?.Invoke(_rotationSpeed);
        }
    }
    public void CopyStats(EnemyStats importedStats)
    { 
        if (importedStats == null) { Debug.LogError("BaseStats missing for Enemy"); return; }
        MaxHp = importedStats.MaxHp;
        CurrentHp = importedStats.CurrentHp;
        DamageMultiplicator = importedStats.DamageMultiplicator;
        BaseSpeed = importedStats.BaseSpeed;
        Speed = importedStats.Speed;
        XpToDrop = importedStats.XpToDrop;

    }
}
