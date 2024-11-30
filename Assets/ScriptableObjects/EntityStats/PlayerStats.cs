using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Entity Stats", menuName = "Entity Stats/PlayerStats")]
public class PlayerStats : EntityStats
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    [SerializeField] private float _recoveryStaminaSpeed;
    [SerializeField] private float _weaponSize;
    [SerializeField] private float _chargeGain;
    [SerializeField] private float _currentBloodflow;
    [SerializeField] private float _maxBloodflow;
    [SerializeField] private int _level;
    [SerializeField] private int _experiencePoints;

    // Eventos
   
    [HideInInspector] public Action<float> OnAttackSpeedChange;
    [HideInInspector] public Action<float> OnMaxStaminaChange;
    [HideInInspector] public Action<float> OnStaminaChange;
    [HideInInspector] public Action<float> OnRecoveryStaminaSpeedChange;
    [HideInInspector] public Action<float> OnWeaponSizeChange;
    [HideInInspector] public Action<float> OnBloodflowMultiplierChanged;
    [HideInInspector] public Action<float> OnCurrentBloodFlowChange;
    [HideInInspector] public Action<float> OnMaxBloodFlowChange;
    [HideInInspector] public Action<int> OnPlayerLevelChange;
    [HideInInspector] public Action<int> OnPayerExperiencePointsChange;

  

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
            OnMaxStaminaChange?.Invoke(_maxStamina);
        }
    }
    public float CurrentStamina
    {
        get => _currentStamina;
        set
        {
            if (Mathf.Approximately(_currentStamina, value)) return;
            _currentStamina = value;
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

    public float BloodflowMultiplier
    {
        get => _chargeGain;
        set
        {
            _chargeGain = value;
            OnBloodflowMultiplierChanged?.Invoke(_chargeGain);
        }
    }
    public float CurrentBloodFlow
    {
        get => _currentBloodflow;
        set
        {
            _currentBloodflow = value;
            OnCurrentBloodFlowChange?.Invoke(_currentBloodflow);
        }
    }
    public float MaxBloodFlow
    {
        get => _maxBloodflow;
        set
        {
            _maxBloodflow = value;
            OnMaxBloodFlowChange?.Invoke(_maxBloodflow);
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
        BloodflowMultiplier = importedData.BloodflowMultiplier;
        Level = importedData.Level;
        ExperiencePoints = importedData.ExperiencePoints;
    }
}
