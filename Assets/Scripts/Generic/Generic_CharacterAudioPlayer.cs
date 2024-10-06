using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generic_CharacterAudioPlayer : MonoBehaviour
{
    [Range(-1, 1)][SerializeField] float basePitchModifier;
    [SerializeField] Generic_EventSystem eventSystem;

    [SerializeField] AudioClip SwordSwingSFX;
    [SerializeField] AudioClip SwordHitSFX;
    [SerializeField] AudioClip GetHitSFX;
    [SerializeField] AudioClip DeathSFX;
    [SerializeField] AudioClip GotParriedSFX;
    [SerializeField] AudioClip SuccesfullParrySFX;
    SFX_PlayerSingleton SFX_Player;
    public virtual void OnEnable()
    {
        SFX_Player = SFX_PlayerSingleton.Instance;
        eventSystem.OnShowCollider += playSwordSwing;
        eventSystem.OnDealtDamage += playSwordHit;
        eventSystem.OnReceiveDamage += playGetHit;
        eventSystem.OnDeath += playDeath;
        eventSystem.OnGettingParried += playParried;
        eventSystem.OnSuccessfulParry += playSuccesfullParry;
    }
    public virtual void OnDisable()
    {
        eventSystem.OnShowCollider -= playSwordSwing;
        eventSystem.OnDealtDamage -= playSwordHit;
        eventSystem.OnReceiveDamage -= playGetHit;
        eventSystem.OnDeath -= playDeath;
        eventSystem.OnGettingParried -= playParried;
        eventSystem.OnSuccessfulParry -= playSuccesfullParry; 
    }
    void playSwordSwing()
    {
        SFX_Player.playSFX(SwordSwingSFX,0.1f,0,basePitchModifier);
    }
    void playSwordHit(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        SFX_Player.playSFX(SwordHitSFX, 0.2f, 0, basePitchModifier);
    }
    void playGetHit(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        SFX_Player.playSFX(GetHitSFX, 0.2f, 0, basePitchModifier);
    }
    void playDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        SFX_Player.playSFX(DeathSFX, 0.2f);
    }
    void playParried(int i)
    {
        SFX_Player.playSFX(GotParriedSFX,0.1f, 0, basePitchModifier);
    }
    void playSuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        SFX_Player.playSFX(SuccesfullParrySFX);
    }
}
