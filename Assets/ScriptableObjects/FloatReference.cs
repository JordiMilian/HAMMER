using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;
    public Action OnValueChanged;
    public float GetValue()
    {
        if (UseConstant) { return ConstantValue; }
        else { return Variable.Value; }
    }
    public void ChangeValue(float value)
    {
        if (UseConstant) { ConstantValue = value; }
        else { Variable.SetValue(value);}

        OnValueChanged?.Invoke();
    }
}

