using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera CMVC;
    private CinemachineBasicMultiChannelPerlin CMVCx;
    float ShakeTimer;
    bool Shaking = false;

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
