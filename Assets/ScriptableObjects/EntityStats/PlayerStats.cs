using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Entity Stats", menuName = "Entity Stats/PlayerStats")]
public class PlayerStats : EntityStats
{
    [SerializeField] private float _currentHp;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _stamina;
    [SerializeField] private float _recoveryStaminaSpeed;
    [SerializeField] private float _weaponSize;
    [SerializeField] private float _chargeGain;
    [SerializeField] private int _playerLevel;
    [SerializeField] private int _playerExperiencePoints;

    // Eventos
    [HideInInspector] public UnityEvent<float> OnCurrentHpChange;
    [HideInInspector] public UnityEvent<float> OnAttackSpeedChange;
    [HideInInspector] public UnityEvent<float> OnStaminaChange;
    [HideInInspector] public UnityEvent<float> OnRecoveryStaminaSpeedChange;
    [HideInInspector] public UnityEvent<float> OnWeaponSizeChange;
    [HideInInspector] public UnityEvent<float> OnChargeGainChange;
    [HideInInspector] public UnityEvent<float> OnPlayerLevelChange;
    [HideInInspector] public UnityEvent<float> OnPayerExperiencePointsChange;

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

    public float Stamina
    {
        get => _stamina;
        set
        {
            if (Mathf.Approximately(_attackSpeed, value)) return;
            _stamina = value;
            OnStaminaChange?.Invoke(_stamina);
        }
    }

    public float RecoveryStaminaSpeed
    {
        get => _recoveryStaminaSpeed;
        set
        {
            if (Mathf.Approximately(_attackSpeed, value)) return;
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

    public float PlayerLevel
    {
        get => _playerLevel;
        set
        {
            //_playerLevel = value;
            OnPlayerLevelChange?.Invoke(_playerLevel);
        }
    }

    public float PlayerExperiencePoints
    {
        get => _playerExperiencePoints;
        set
        {
            //_playerExperiencePoints = value;
            OnPayerExperiencePointsChange?.Invoke(_playerExperiencePoints);
        }
    }
    public void CopyData(PlayerStats importedData) //Funcion que simplemente copia TODAS las variables de una función a otra
    {
        MaxHp = importedData.MaxHp;
        Speed = importedData.Speed;
        DamageMultiplicator = importedData.DamageMultiplicator;
        CurrentHp = importedData.CurrentHp;
        AttackSpeed = importedData.AttackSpeed;
        Stamina = importedData.Stamina;
        RecoveryStaminaSpeed = importedData.RecoveryStaminaSpeed;
        WeaponSize = importedData.WeaponSize;
        ChargeGain = importedData.ChargeGain;
        PlayerLevel = importedData.PlayerLevel;
        PlayerExperiencePoints = importedData.PlayerExperiencePoints;
    }
}
