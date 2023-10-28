using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera CMVC;
    private CinemachineBasicMultiChannelPerlin CMVCx;
   
    float ShakeTimer;
    bool Shaking = false;

    private void Start()
    {
        CMVC = GetComponent<CinemachineVirtualCamera>();
    }
    public void ShakeCamera(float Intensity, float Time)
    {
        CMVCx = CMVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ShakeTimer = Time;
        Shaking = true;
        CMVCx.m_AmplitudeGain = Intensity;
    }
    void StopCamera()
    {
        CMVCx.m_AmplitudeGain = 0;
        
    }
    private void Update()
    {
        if (Shaking == true)
        {
            ShakeTimer = ShakeTimer -  Time.deltaTime;
            if (ShakeTimer <= 0)
            {
                StopCamera();
                Shaking = false;
            }
        }
        
    }
}
