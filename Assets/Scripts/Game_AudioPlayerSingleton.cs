using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class Game_AudioPlayerSingleton : MonoBehaviour
{
    public static Game_AudioPlayerSingleton Instance;
    [SerializeField] AudioSource audioSource;
    Dictionary<string,AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    [SerializeField] List<AudioClip> audioClipsList = new List<AudioClip>();
    float basePitch;
    float baseVolume;

   
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
        /*
        foreach (AudioClip Clip in audioClipsList)
        {
            string clipName = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Clip)); //Get the name of the asset somehow
            Debug.Log(Clip.name);
            sfxDictionary.Add(clipName, Clip);
        }
        */
        basePitch = audioSource.pitch;
        baseVolume = audioSource.volume;


    }
    public void playSFX(string key)
    {
        if(sfxDictionary.ContainsKey(key)) 
        {
            audioSource.clip = sfxDictionary[key];
            audioSource.Play();
            return;
        }
        Debug.LogWarning("Couldn't find an audio clip named: " + key);
    }
    public void playSFXclip(AudioClip clip, float pitchVariationAdder = 0,float addedVolum = 0, float addedPitch = 0)
    {
        audioSource.pitch = basePitch;
        audioSource.volume = baseVolume;
        float randomAdder = UnityEngine.Random.Range(-pitchVariationAdder, pitchVariationAdder);
        audioSource.pitch = basePitch + randomAdder;
        audioSource.pitch += addedPitch;
        audioSource.volume += addedVolum;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
