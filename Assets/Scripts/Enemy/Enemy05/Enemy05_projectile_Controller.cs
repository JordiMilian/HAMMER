using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy05_projectile_Controller : MonoBehaviour, IDamageDealer
{
    public Action<DealtDamageInfo> OnDamageDealt_event { get ; set ; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
    private void Awake()
    {
        Destroy(gameObject, 1);
    }
}
