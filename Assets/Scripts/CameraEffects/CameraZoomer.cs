using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Player_FollowMouse_withFocus followMouse;
    [Header("Base info")]
    [SerializeField] float BaseZoom;
    [SerializeField] float BaseDuration;
    [Header("Read only")]
    public float CurrentZoom;
    public float TargetZoom;
    Coroutine currentCoroutine;
    [Header("Focus info")]
    bool isFocusingZoom;
    public float FocusZoom;
    [Serializable]
    public class ZoomInfo
    {
        public float ZoomSize;
        public float ZoomDuration;
        public string Name;

        public ZoomInfo(float zoom, float adder, string name)
        {
            ZoomSize = zoom;
            ZoomDuration = adder;
            Name = name;
        }
    }
    public List<ZoomInfo> zoomInfos = new List<ZoomInfo>();

    private void Start()
    {
        ZoomInfo BaseInfo = new ZoomInfo(BaseZoom, BaseDuration, "Base");
        AddZoomInfo(BaseInfo);
    }
    public void AddZoomInfo(ZoomInfo info)
    {
        StopAllZoomCoroutines();
       
        zoomInfos.Add(info);
        UpdateNewCoroutine();
    }
    public void RemoveZoomInfo(string name)
    {       
         StopAllZoomCoroutines();
            
        ZoomInfo infoToRemove = new ZoomInfo(0, 0, "null");
        foreach (ZoomInfo info in zoomInfos)
        {
            if (info.Name == name)
            {
                infoToRemove = info;
            }
        }
        zoomInfos.Remove(infoToRemove);
        UpdateNewCoroutine();
        
    }
    ZoomInfo CheckLatestZoomInfo()
    {
        if (zoomInfos.Count > 0)
        {
            return zoomInfos.Last();
        }
        else return null;
    }
    public void UpdateNewCoroutine()
    {
        if (!followMouse.IsFocusingEnemy)
        {
            ZoomInfo Latest = CheckLatestZoomInfo();
            TargetZoom = Latest.ZoomSize;
            if (CurrentZoom != TargetZoom)
            {
                currentCoroutine = StartCoroutine(ChangeZoomSmoothly(Latest));
            }
        }
           
        
    }
    IEnumerator ChangeZoomSmoothly(ZoomInfo info)
    {
        float timer = 0;
        CurrentZoom = virtualCamera.m_Lens.OrthographicSize;
        while (timer < info.ZoomDuration)
        {
            timer += Time.deltaTime;
            CurrentZoom = Mathf.Lerp(CurrentZoom, info.ZoomSize, timer / info.ZoomDuration);
            virtualCamera.m_Lens.OrthographicSize = CurrentZoom;

            yield return null;
        }
        CurrentZoom = info.ZoomSize;
    }

    Coroutine FocusInCor, FocusOutCor;
    public void StartFocusInTransition()
    {
       StopAllZoomCoroutines();
        FocusInCor = StartCoroutine(TransitionToFocus());
    }
    public void StartFocusOutTransition()
    {
        StopAllZoomCoroutines();
        isFocusingZoom = false;
        FocusOutCor = StartCoroutine(ChangeZoomSmoothly(CheckLatestZoomInfo()));
    }

    IEnumerator TransitionToFocus()
    {
        CurrentZoom = virtualCamera.m_Lens.OrthographicSize;

        float timer = 0;
        while (timer < 2)
        {
            timer += Time.deltaTime;
            CurrentZoom = Mathf.Lerp(CurrentZoom, FocusZoom, timer / 2);
            virtualCamera.m_Lens.OrthographicSize = CurrentZoom;
            yield return null;
        }
        isFocusingZoom = true;
    }
    
    private void Update()
    {
        if (isFocusingZoom) { ConstantlyChangeZoom(); }
    }
    void ConstantlyChangeZoom()
    {
        virtualCamera.m_Lens.OrthographicSize = FocusZoom;
    }
    void StopAllZoomCoroutines()
    {
        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        if (FocusOutCor != null) { StopCoroutine(FocusOutCor); }
        if (FocusInCor != null) { StopCoroutine(FocusInCor); }
    }
}
