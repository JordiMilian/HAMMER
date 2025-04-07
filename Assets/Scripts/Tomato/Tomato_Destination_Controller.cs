using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Tomato_Destination_Controller : MonoBehaviour, IDamageDealer
{
    [SerializeField] VisualEffect TomatoSplash;
    public void EV_PlaySplash()
    {
        TomatoSplash.Play();
    }
    [SerializeField] Collider2D DamageCollider;
    public void EV_Enemy_ShowAttackCollider()
    {
        DamageCollider.enabled = true;
    }
    public void EV_Enemy_HideAttackCollider()
    {
        DamageCollider.enabled = false;
    }

    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
}
