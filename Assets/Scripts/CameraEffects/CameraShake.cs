using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera CMVC;
    private CinemachineBasicMultiChannelPerlin CMVCx;

    public static CameraShake Instance;
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

        CMVCx = CMVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float Intensity, float Time)
    {
        StartCoroutine(ShakeCoroutine(Intensity, Time));
    }
    IEnumerator ShakeCoroutine(float Intensity, float sTime)
    {
        float timer = 0;
        while(timer < sTime)
        {
            timer += Time.deltaTime * Time.timeScale;

            CMVCx.m_AmplitudeGain = Intensity * Time.timeScale; //multiply intensity with timescale so when the game pauses it stops

            yield return null;
        }

        CMVCx.m_AmplitudeGain = 0;
    }
}
