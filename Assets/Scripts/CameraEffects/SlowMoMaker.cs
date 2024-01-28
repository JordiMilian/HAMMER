using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowMoMaker : MonoBehaviour
{
    public static SlowMoMaker Instance;
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
    }

    public void CallSlowMo(float SlowPercent, float DurationSeconts)
    { StartCoroutine(SlowMoCorroutine(SlowPercent, DurationSeconts)); }

    IEnumerator SlowMoCorroutine(float SlowPercent, float DurationSeconts)
    {
        float lerpedPercent = Mathf.InverseLerp(100, 0, SlowPercent);
        Debug.Log(lerpedPercent);
        Time.timeScale = lerpedPercent;
        yield return new WaitForSecondsRealtime(DurationSeconts);
        Time.timeScale = 1;
        StartCoroutine(SlowMoFadeOut(DurationSeconts / 2, lerpedPercent));
    }
    IEnumerator SlowMoFadeOut(float FadeOutSeconds, float startingTimeScale)
    {
        float timer = 0;
        while (timer< FadeOutSeconds)
        {
            timer += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(1, startingTimeScale, 1 / FadeOutSeconds * timer);
            yield return null;
        }
    }
}
