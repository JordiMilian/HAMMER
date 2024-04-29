using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AudioPlayer : Generic_CharacterAudioPlayer
{
    [SerializeField] Enemy_EventSystem enemyEvents;
    [SerializeField] AudioClip ThrowTomatoSFX;
    [SerializeField] AudioClip ThrowGreenProjectileSFX;
    private void OnEnable()
    {
        base.OnEnable();
        enemyEvents.OnThrowTomato += playThrowTomato;
        enemyEvents.OnThrowGreenProjectile += playThrowGreenProjectile;
    }
    private void OnDisable()
    {
        base.OnDisable();
        enemyEvents.OnThrowTomato += playThrowTomato;
        enemyEvents.OnThrowGreenProjectile += playThrowGreenProjectile;
    }
    void playThrowTomato()
    {
        playSFX(ThrowTomatoSFX);
    }
    void playThrowGreenProjectile()
    {
        playSFX(ThrowGreenProjectileSFX, 0.1f);
    }
}
