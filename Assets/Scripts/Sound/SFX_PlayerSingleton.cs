
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFX_PlayerSingleton : MonoBehaviour
{
    [SerializeField] GameState gameState;
    List<AudioSource> audioSourcesList = new List<AudioSource>();

    public static SFX_PlayerSingleton Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        audioSourcesList = GetComponentsInChildren<AudioSource>().ToList<AudioSource>();
    }
    public void playSFX(AudioClip clip, float pitchVariationAdder = 0, float addedVolum = 0, float addedPitch = 0)
    {
        AudioSource audioSource = GetFreeAudioSource();

        if (clip == null) { Debug.LogWarning("Missing audio clip: " + clip.name); return; }
        audioSource.pitch = 1;
        SetGlobalVolume(ref audioSource);

        float randomAdder = Random.Range(-pitchVariationAdder, pitchVariationAdder);
        audioSource.pitch += randomAdder;
        audioSource.pitch += addedPitch;
        audioSource.volume += addedVolum;

        audioSource.clip = clip;
        audioSource.Play();

    }
    AudioSource GetFreeAudioSource()
    {
        AudioSource freeAudioSource = null;

        foreach (AudioSource source in audioSourcesList)
        {
            if (source.isPlaying) { continue; }

            freeAudioSource = source;
            break;
        }

        if(freeAudioSource == null) { Debug.LogWarning("No free audio source available, consider adding more source childs to the singleton"); }

        return freeAudioSource;
    }
    void SetGlobalVolume(ref AudioSource audioSource)
    {
        if (gameState == null)
        {
            Debug.LogWarning("Missing GameState reference to get Global SFX Volume in: " + gameObject.name);
            audioSource.volume = .5f;
        }
        else { audioSource.volume = gameState.SFXVolum; }
    }
}
