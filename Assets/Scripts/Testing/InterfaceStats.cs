using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthStat
{
    public float Health
    {
        get; set;
    }
}
public class InterfaceStats : ScriptableObject, IHealthStat
{
    [SerializeField] float _healthStat;

    public Action<float> onHealthSet;

    public float Health
    {
        get { return _healthStat; }
        set { onHealthSet?.Invoke(value); _healthStat = value; }
    }
}
