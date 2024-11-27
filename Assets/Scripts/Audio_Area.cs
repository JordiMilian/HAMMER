using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Audio_Area : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    Coroutine CurrentFade;
    [HideInInspector] public float BaseVolume;
    [SerializeField] float FadesSeconds;
    [SerializeField] bool isStartingArea;
    [SerializeField] Generic_OnTriggerEnterEvents audioTriggerCollider;
    private void OnEnable()
    {
        audioTriggerCollider.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        audioTriggerCollider.OnTriggerEntered += onFadeInAudio;
        audioTriggerCollider.OnTriggerExited += onFadeOutAudio;
        MusicManager.Instance.AddMusicSource(audioSource);
        BaseVolume = audioSource.volume;
    }
    private void OnDisable()
    {
        audioTriggerCollider.OnTriggerEntered -= onFadeInAudio;
        audioTriggerCollider.OnTriggerExited -= onFadeOutAudio;
        MusicManager.Instance.RemoveMusicSource(audioSource);
    }
    private void Start()
    {
        if (isStartingArea) { FadeIn(0.5f); }
        else { FadeOut(0); }
    }

    public void onFadeInAudio(Collider2D collision)
    {
        if (CurrentFade != null) { StopCoroutine(CurrentFade); }
        CurrentFade = StartCoroutine(FadeIn(FadesSeconds));
    }
    public void onFadeOutAudio(Collider2D collision)
    {
        if (CurrentFade != null) { StopCoroutine(CurrentFade); }
        CurrentFade = StartCoroutine(FadeOut(FadesSeconds));
    }
    IEnumerator FadeOut(float time)
    { 
         float timer = 0;
        float startingVolume = audioSource.volume;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(startingVolume, 0, timer/time);
            audioSource.volume = newVolume;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }

    IEnumerator FadeIn(float time)
    {
        audioSource.Play();
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(0, BaseVolume, timer / time);
            audioSource.volume = newVolume;
            yield return null;
        }
        audioSource.volume = BaseVolume;
       
    }
}
