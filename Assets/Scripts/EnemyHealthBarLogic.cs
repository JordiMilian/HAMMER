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
        eventSystem.OnUpdatedHealth += UpdateSize;
    }
    void UpdateSize()
    {
        float newSize = Mathf.InverseLerp(0, healthSystem.MaxHP.Value, healthSystem.CurrentHP.Value);
        HealthBarSize1.localScale = new Vector3(newSize, 1, 1);
    }
}
