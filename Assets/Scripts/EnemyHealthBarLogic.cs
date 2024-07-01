using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarLogic : MonoBehaviour
{
    [SerializeField] Generic_HealthSystem healthSystem;
    [SerializeField] Generic_EventSystem eventSystem;
    [SerializeField] Transform HealthBarSize1;
    private void OnEnable()
    {
        healthSystem.CurrentHP.OnValueChanged += UpdateSize;
        healthSystem.CurrentHP.Variable.OnValueSet += UpdateSize;
    }
    void UpdateSize()
    {
        float newSize = Mathf.InverseLerp(0, healthSystem.MaxHP.GetValue(), healthSystem.CurrentHP.GetValue());
        if(HealthBarSize1 != null)
        {
            HealthBarSize1.localScale = new Vector3(newSize, 1, 1);
        }
        
    }
}
