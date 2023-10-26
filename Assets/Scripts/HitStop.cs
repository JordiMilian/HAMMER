using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;
    public void Stop(float StopSeconds)
    {
        if (waiting == false)
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
        Time.timeScale = 1;
        waiting = false;
    }
    IEnumerator PreWait()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 0;
    }

}
