using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generic_CharacterAudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Generic_EventSystem eventSystem;
    float basePitch;
    float baseVolume;

    [SerializeField] AudioClip SwordSwingSFX;
    [SerializeField] AudioClip SwordHitSFX;
    [SerializeField] AudioClip GetHitSFX;
    [SerializeField] AudioClip DeathSFX;
    [SerializeField] AudioClip GotParriedSFX;
    [SerializeField] AudioClip SuccesfullParrySFX;
    private void Awake()
    {
        basePitch = audioSource.pitch;
        baseVolume = audioSource.volume;
    }
    public void OnEnable()
    {
        eventSystem.OnShowCollider += playSwordSwing;
        eventSystem.OnDealtDamage += playSwordHit;
        eventSystem.OnReceiveDamage += playGetHit;
        eventSystem.OnDeath += playDeath;
        eventSystem.OnGettingParried += playParried;
        eventSystem.OnSuccessfulParry += playSuccesfullParry;
    }
    public void OnDisable()
    {
        eventSystem.OnShowCollider -= playSwordSwing;
        eventSystem.OnDealtDamage -= playSwordHit;
        eventSystem.OnReceiveDamage -= playGetHit;
        eventSystem.OnDeath -= playDeath;
        eventSystem.OnGettingParried -= playParried;
        eventSystem.OnSuccessfulParry -= playSuccesfullParry;
          
    }
    public void playSFX(AudioClip clip, float pitchVariationAdder = 0, float addedVolum = 0, float addedPitch = 0)
    {
        if(clip == null) { Debug.LogWarning("Missing audio clip: "+ clip.name); return; }
        audioSource.pitch = basePitch;
        audioSource.volume = baseVolume;
        float randomAdder = Random.Range(-pitchVariationAdder, pitchVariationAdder); 
        audioSource.pitch = basePitch + randomAdder;
        audioSource.pitch += addedPitch;
        audioSource.volume += addedVolum;

        audioSource.clip = clip;
        audioSource.Play();
    }
    void playSwordSwing()
    {
        playSFX(SwordSwingSFX,0.1f);
    }
    void playSwordHit(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        playSFX(SwordHitSFX, 0.2f);
    }
    void playGetHit(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        playSFX(GetHitSFX, 0.2f);
    }
    void playDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        Game_AudioPlayerSingleton.Instance.playSFXclip(DeathSFX);
    }
    void playParried()
    {
        playSFX(GotParriedSFX,0.1f);
    }
    void playSuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        playSFX(SuccesfullParrySFX);
    }
}
