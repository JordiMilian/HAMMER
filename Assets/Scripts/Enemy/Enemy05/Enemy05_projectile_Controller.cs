using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy05_projectile_Controller : MonoBehaviour, IDamageDealer, IParryReceiver
{
    public Action<DealtDamageInfo> OnDamageDealt_event { get ; set ; }
    public Action<GettingParriedInfo> OnParryReceived_event { get ; set ; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }

    public void OnParryReceived(GettingParriedInfo info)
    {
        OnParryReceived_event?.Invoke(info);
        Destroy(gameObject);
    }

    private void Awake()
    {
        Destroy(gameObject, 1);
    }
}
