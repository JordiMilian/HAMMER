using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;
    [SerializeField] AudioSource audioSource;
    Dictionary<string,AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    [SerializeField] List<AudioClip> audioClipsList = new List<AudioClip>();


   
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

        foreach (AudioClip Clip in audioClipsList)
        {
            string clipName = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Clip)); //Get the name of the asset somehow
            Debug.Log(Clip.name);
            sfxDictionary.Add(clipName, Clip);
        }
        
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
}
