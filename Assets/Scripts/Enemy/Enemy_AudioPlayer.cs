using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AudioPlayer : Generic_CharacterAudioPlayer
{
    [SerializeField] Enemy_EventSystem enemyEvents;
    [SerializeField] AudioClip ThrowTomatoSFX;
    [SerializeField] AudioClip ThrowGreenProjectileSFX;
    public override void OnEnable()
    {
        base.OnEnable();
        enemyEvents.OnThrowTomato += playThrowTomato;
        enemyEvents.OnThrowGreenProjectile += playThrowGreenProjectile;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        enemyEvents.OnThrowTomato += playThrowTomato;
        enemyEvents.OnThrowGreenProjectile += playThrowGreenProjectile;
    }
    void playThrowTomato()
    {
       SFX_PlayerSingleton.Instance.playSFX(ThrowTomatoSFX);
    }
    void playThrowGreenProjectile()
    {
        SFX_PlayerSingleton.Instance.playSFX(ThrowGreenProjectileSFX, 0.1f);
    }
}
