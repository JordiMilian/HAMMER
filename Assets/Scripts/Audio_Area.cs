using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Audio_Area : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    Coroutine CurrentFade;
    float BaseVolume;
    [SerializeField] float FadesSeconds;
    [SerializeField] bool isStartingArea;
    private void Awake()
    {
        BaseVolume = audioSource.volume;
    }
    private void Start()
    {
        if (isStartingArea) { FadeIn(0.5f); }
        else { FadeOut(0); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            if(CurrentFade != null) { StopCoroutine(CurrentFade); }
            CurrentFade = StartCoroutine( FadeIn(FadesSeconds));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            FadeOut(FadesSeconds);
            if (CurrentFade != null) { StopCoroutine(CurrentFade); }
            CurrentFade = StartCoroutine(FadeOut(FadesSeconds));
        }
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
