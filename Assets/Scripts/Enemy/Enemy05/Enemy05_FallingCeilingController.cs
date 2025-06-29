using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy05_FallingCeilingController : MonoBehaviour, IDamageDealer
{
    [SerializeField] AudioClip fallenSFX;
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
    [SerializeField] VisualEffect fallenVFX;
    public void EV_OnRockLanded()
    {
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.VerySmall);
        SFX_PlayerSingleton.Instance.playSFX(fallenSFX, .2f,-.1f);
        fallenVFX.Play();
    }
}
