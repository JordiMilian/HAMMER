using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    float CurrentZoom;
    public float BaseZoom;
    public float TargetZoom;
   
    public float CurrentZoomSpeed = 0.03f;
    public float BaseZoomSpeed = 0.03f;
    [SerializeField] float ZoomMargin = 0.2f;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        CurrentZoom = virtualCamera.m_Lens.OrthographicSize;
        TargetZoom = CurrentZoom;
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(CurrentZoom - TargetZoom) > ZoomMargin)
        {
            virtualCamera.m_Lens.OrthographicSize = ChangeCameraZoom();
        }
    }

    float ChangeCameraZoom()
    {
        if (CurrentZoom < TargetZoom)
        {
            CurrentZoom += CurrentZoomSpeed;
        }
        if (CurrentZoom > TargetZoom)
        {
            CurrentZoom -= CurrentZoomSpeed;
        }
        return CurrentZoom;
    }
}
