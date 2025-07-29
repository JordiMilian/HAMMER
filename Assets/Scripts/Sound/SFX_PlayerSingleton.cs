
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_PlayerSingleton : MonoBehaviour
{
    //[SerializeField] GameState gameState;
    List<AudioSource> audioSourcePool = new List<AudioSource>();
    [SerializeField] private int initialPoolSize = 5;

    [SerializeField] AudioMixerGroup SFXGround, MusicGroup;

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
        InitializeInitialAudioSourcePool();

        //
        void InitializeInitialAudioSourcePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewAudioSource();
            }
        }
    }

    AudioSource CreateNewAudioSource()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        audioSourcePool.Add(newAudioSource); 
        return newAudioSource;
    }
    AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying) // Si no está reproduciendo, está disponible
            {
                return source;
            }
        }
        return CreateNewAudioSource();
    }

    public AudioSource playSFX(AudioClip clip, float pitchVariationAdder = 0, float addedVolum = 0, float addedPitch = 0)//added volum should be a percent probably
    {
        if (clip == null) { Debug.LogWarning("Missing audio clip: " + clip.name); return null; }

        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = SFXGround;
        audioSource.pitch = 1;
        audioSource.volume = 1;
        audioSource.loop = false;

        float randomAdder = Random.Range(-pitchVariationAdder, pitchVariationAdder);
        audioSource.pitch += randomAdder;
        audioSource.pitch += addedPitch;
        audioSource.volume += addedVolum;

        audioSource.clip = clip;
        audioSource.Play();

        return audioSource;
        //
    }
    public AudioSource FadeInMusic(AudioClip musicClip, float fadeTime = 1.5f)
    {
        if (musicClip == null) { Debug.LogWarning("Missing audio clip: " + musicClip.name); return null; }

        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = MusicGroup;
        audioSource.pitch = 1; audioSource.volume = 1;
        audioSource.loop = true;

        audioSource.clip = musicClip;
        StartCoroutine(UsefullMethods.FadeIn(audioSource, fadeTime, 1));

        return audioSource;
    }
    public void FadeOutMusic(AudioSource playingSource, float fadeTime = 1)
    {
        foreach(AudioSource source in audioSourcePool)
        {
            if (source == playingSource && source.isPlaying)
            {
                StartCoroutine(UsefullMethods.FadeOut(source, fadeTime));
            }
        }
    }

}
