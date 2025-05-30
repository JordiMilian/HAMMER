using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntensitiesEnum
{
    VerySmall, Small, Medium, Big, VeryBig
}
public class TimeScaleEditor : MonoBehaviour
{
    public static TimeScaleEditor Instance;

    //SINGLETON STUFF
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

    float BaseScale = 1f;
    public void SlowMotion(IntensitiesEnum intensity)
    {
        switch (intensity)
        {
            case IntensitiesEnum.VerySmall:
                StartCoroutine(SlowMoCorroutine(70,.5f));
                break;
            case IntensitiesEnum.Small:
                StartCoroutine(SlowMoCorroutine(80,0.75f));
                break;
            case IntensitiesEnum.Medium:
                StartCoroutine(SlowMoCorroutine(85,1f));
                break;
            case IntensitiesEnum.Big:
                StartCoroutine(SlowMoCorroutine(90,1.5f));
                break;
            case IntensitiesEnum.VeryBig:
                StartCoroutine(SlowMoCorroutine(90,2));
                break;
        }
    }

    IEnumerator SlowMoCorroutine(float SlowPercent, float DurationSeconts)
    {
        float lerpedPercent = Mathf.InverseLerp(100, 0, SlowPercent);
        BaseScale = lerpedPercent;
        Time.timeScale = lerpedPercent;
        //Time.fixedDeltaTime = lerpedPercent * 0.02f;
        yield return new WaitForSecondsRealtime(DurationSeconts);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        BaseScale = 1;
    }

    bool waiting;
    public void HitStop(IntensitiesEnum intensity)
    {
        if (!waiting)
        {
            switch (intensity)
            {
                case IntensitiesEnum.VerySmall:
                    StartCoroutine(Hitstopper(0.05f));
                    break;
                case IntensitiesEnum.Small:
                    StartCoroutine(Hitstopper(0.1f));
                    break;
                case IntensitiesEnum.Medium:
                    StartCoroutine(Hitstopper(0.15f));
                    break;
                case IntensitiesEnum.Big:
                    StartCoroutine(Hitstopper(0.2f));
                    break;
                case IntensitiesEnum.VeryBig:
                    StartCoroutine(Hitstopper(0.3f));
                    break;
            }
        }
    }

    IEnumerator Hitstopper(float StopSeconds)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(0.01f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(StopSeconds);
        Time.timeScale = BaseScale;
        waiting = false;
    }
}
