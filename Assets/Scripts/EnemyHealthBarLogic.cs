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
    void UpdateSize(object sender, EventArgs args)
    {
        float newSize = Mathf.InverseLerp(0, healthSystem.MaxHealth, healthSystem.CurrentHealth);
        HealthBarSize1.localScale = new Vector3(newSize, 1, 1);
    }
}
