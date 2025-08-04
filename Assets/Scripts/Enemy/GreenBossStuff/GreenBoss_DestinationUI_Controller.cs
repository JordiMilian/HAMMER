using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GreenBoss_DestinationUI_Controller : MonoBehaviour, IDamageDealer, IParryReceiver
{
    [SerializeField] AudioClip SFX_Landed;
    [SerializeField] VisualEffect VFX_Landed;
    public Action<DealtDamageInfo> OnDamageDealt_event { get ; set; }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
    public void EV_LandedSFX()
    {
        SFX_PlayerSingleton.Instance.playSFX(SFX_Landed, 0.1f);
        VFX_Landed.Play();
    }
    public void OnParryReceived(GettingParriedInfo info)
    {
        OnParryReceived_event?.Invoke(info);
    }
}
