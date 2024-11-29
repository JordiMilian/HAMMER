using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Entity Stats", menuName = "Entity Stats/PlayerStats")]
public class PlayerStats : EntityStats
{
    [SerializeField] private float _currentHp;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    [SerializeField] private float _recoveryStaminaSpeed;
    [SerializeField] private float _weaponSize;
    [SerializeField] private float _chargeGain;
    [SerializeField] private int _level;
    [SerializeField] private int _experiencePoints;

    // Eventos
    [HideInInspector] public Action<float> OnCurrentHpChange;
    [HideInInspector] public Action<float> OnAttackSpeedChange;
    [HideInInspector] public Action<float> OnStaminaChange;
    [HideInInspector] public Action<float> OnRecoveryStaminaSpeedChange;
    [HideInInspector] public Action<float> OnWeaponSizeChange;
    [HideInInspector] public Action<float> OnChargeGainChange;
    [HideInInspector] public Action<int> OnPlayerLevelChange;
    [HideInInspector] public Action<int> OnPayerExperiencePointsChange;

    public float CurrentHp
    {
        get => _currentHp;
        set
        {
            if (Mathf.Approximately(_currentHp, value)) return;
            _currentHp = value;
            OnCurrentHpChange?.Invoke(_currentHp);
        }
    }

    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            if (Mathf.Approximately(_attackSpeed, value)) return;
            _attackSpeed = value;
            OnAttackSpeedChange?.Invoke(_attackSpeed);
        }
    }

    public float MaxStamina
    {
        get => _maxStamina;
        set
        {
            if (Mathf.Approximately(_maxStamina, value)) return;
            _maxStamina = value;
            OnStaminaChange?.Invoke(_maxStamina);
        }
    }
    public float CurrentStamina
    {
        get => _currentStamina;
        set
        {
            if (Mathf.Approximately(_currentStamina, value)) return;
            _maxStamina = value;
            OnStaminaChange?.Invoke(_currentStamina);
        }
    }

    public float RecoveryStaminaSpeed
    {
        get => _recoveryStaminaSpeed;
        set
        {
            if (Mathf.Approximately(_recoveryStaminaSpeed, value)) return;
            _recoveryStaminaSpeed = value;
            OnRecoveryStaminaSpeedChange?.Invoke(_recoveryStaminaSpeed);
        }
    }

    public float WeaponSize
    {
        get => _weaponSize;
        set
        {
            _weaponSize = value;
            OnWeaponSizeChange?.Invoke(_weaponSize);
        }
    }

    public float ChargeGain
    {
        get => _chargeGain;
        set
        {
            _chargeGain = value;
            OnChargeGainChange?.Invoke(_chargeGain);
        }
    }

    public int Level
    {
        get => _level;
        set
        {
            //_playerLevel = value;
            OnPlayerLevelChange?.Invoke(_level);
        }
    }

    public int ExperiencePoints
    {
        get => _experiencePoints;
        set
        {
            //_playerExperiencePoints = value;
            OnPayerExperiencePointsChange?.Invoke(_experiencePoints);
        }
    }
    public void CopyData(PlayerStats importedData) //Funcion que simplemente copia TODAS las variables de una función a otra
    {
        MaxHp = importedData.MaxHp;
        Speed = importedData.Speed;
        DamageMultiplicator = importedData.DamageMultiplicator;
        CurrentHp = importedData.CurrentHp;
        AttackSpeed = importedData.AttackSpeed;
        MaxStamina = importedData.MaxStamina;
        RecoveryStaminaSpeed = importedData.RecoveryStaminaSpeed;
        WeaponSize = importedData.WeaponSize;
        ChargeGain = importedData.ChargeGain;
        Level = importedData.Level;
        ExperiencePoints = importedData.ExperiencePoints;
    }
}
