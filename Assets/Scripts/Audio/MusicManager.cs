using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] bool setVolumeTrigger;
    public GameState gameState;

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
        if(Input.GetKey(KeyCode.M)) 
        { 
            if(Input.GetKeyDown(KeyCode.O))
            {
                gameState.MusicVolum += 0.1f;
                UpdateMusicsVolumes();
            }
            else if(Input.GetKeyDown(KeyCode.L)) 
            {
                gameState.MusicVolum -= 0.1f;
                UpdateMusicsVolumes();
            }
            if(gameState.MusicVolum < 0) { gameState.MusicVolum = 0; }
            if(gameState.MusicVolum > 1) { gameState.MusicVolum = 1; }
        }
        if (Input.GetKey(KeyCode.N))
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                gameState.SFXVolum += 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                gameState.SFXVolum -= 0.1f;
            }
            if(gameState.SFXVolum < 0) { gameState.SFXVolum = 0; }
            if(gameState.SFXVolum > 1) { gameState.SFXVolum = 1; }
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
        newSource.audioSource.volume = gameState.MusicVolum;

        musicSources.Add(newSource);
        Debug.Log("Added new music source with base: "+ newSource.BaseVolume);
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
        
        //float equivalentVolume = Mathf.Lerp(0, mSource.BaseVolume, GeneralMusicVolume);
        mSource.audioSource.volume = gameState.MusicVolum;
        //mSource.audioSource.gameObject.GetComponent<Audio_Area>().BaseVolume = equivalentVolume;
        Debug.Log("Settet new music volume to: " + gameState.MusicVolum);   
    }
    public float GetMusicVolume() { return gameState.MusicVolum; }
}
