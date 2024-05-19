using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float GeneralMusicVolume;
    [SerializeField] bool setVolumeTrigger;

    public List<musicSource> musicSources = new List<musicSource>();
    public class musicSource
    {
        public float BaseVolume;
        public AudioSource audioSource;
    }

    //SINGLETON
    public static MusicManager Instance;
    private void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(setVolumeTrigger)
        {
            UpdateMusicsVolumes();
            setVolumeTrigger = false;
        }
    }
    private void Start()
    {
        UpdateMusicsVolumes();
    }
    void UpdateMusicsVolumes()
    {
        foreach (musicSource source in musicSources)
        {
            SetMusicVolume(source);
        }
    }
    public void AddMusicSource(AudioSource source)
    {
        musicSource newSource = new musicSource();
        newSource.audioSource = source;
        newSource.BaseVolume = source.volume;

        musicSources.Add(newSource);
    }
    public void RemoveMusicSource(AudioSource source)
    {
        musicSource sourceToRemove = new musicSource();

        foreach (var musicSource in musicSources)
        {
            if(musicSource.audioSource == source) { sourceToRemove = musicSource; break; }
        }
        musicSources.Remove(sourceToRemove);
    }
    void SetMusicVolume(musicSource mSource)
    {
        float equivalentVolume = Mathf.Lerp(0, mSource.BaseVolume, GeneralMusicVolume);
        mSource.audioSource.volume = equivalentVolume;
        mSource.audioSource.gameObject.GetComponent<Audio_Area>().BaseVolume = equivalentVolume;
    }
}
