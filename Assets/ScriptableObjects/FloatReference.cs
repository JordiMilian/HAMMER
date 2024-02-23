using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;
    public float Value
    {
        get 
        { 
            //return UseConstant ? ConstantValue : Variable.Value;
            if (UseConstant) { return ConstantValue; }
            else { return Variable.Value; }
        }
        set 
        {
            if (UseConstant) { ConstantValue = value; }
            else { Variable.Value = value; }
        }
    }
}

