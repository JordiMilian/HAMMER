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
    public void ShakeCamera(IntensitiesEnum intensity)
    {
        switch (intensity)
        {
            case IntensitiesEnum.VerySmall:
                StartCoroutine(ShakeCoroutine(.2f,.05f));
                break;
            case IntensitiesEnum.Small:
                StartCoroutine(ShakeCoroutine(.3f, .1f));
                break;
            case IntensitiesEnum.Medium:
                StartCoroutine(ShakeCoroutine(.4f, .15f));
                break;
            case IntensitiesEnum.Big:
                StartCoroutine(ShakeCoroutine(.5f, .2f));
                break;
            case IntensitiesEnum.VeryBig:
                StartCoroutine(ShakeCoroutine(.6f, .25f));
                break;
        }
    }
    IEnumerator ShakeCoroutine(float Intensity, float sTime)
    {
        float timer = 0;
        while(timer < sTime)
        {
            timer += Time.deltaTime;

            CMVCx.m_AmplitudeGain = Intensity; //multiply intensity with timescale so when the game pauses it stops

            yield return null;
        }

        CMVCx.m_AmplitudeGain = 0;
    }
}
