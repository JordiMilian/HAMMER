using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleEditor : MonoBehaviour
{
    public static TimeScaleEditor Instance;

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
    float BaseScale = 1;
    public void SlowMotion(float SlowPercent, float DurationSeconts)
    { StartCoroutine(SlowMoCorroutine(SlowPercent, DurationSeconts)); }

    IEnumerator SlowMoCorroutine(float SlowPercent, float DurationSeconts)
    {
        float lerpedPercent = Mathf.InverseLerp(100, 0, SlowPercent);
        Debug.Log(lerpedPercent);
        BaseScale = lerpedPercent;
        Time.timeScale = lerpedPercent;
        yield return new WaitForSecondsRealtime(DurationSeconts);
        Time.timeScale = 1;
        BaseScale = 1;
    }

    bool waiting;
    public void HitStop(float StopSeconds)
    {
        if (!waiting)
        {
            StartCoroutine(Wait(StopSeconds));
        }
    }
    IEnumerator Wait(float StopSeconds)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(0.01f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(StopSeconds);
        Time.timeScale = BaseScale;
        waiting = false;
    }
}
