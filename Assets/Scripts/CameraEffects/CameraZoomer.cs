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
    [Header("Base info")]
    [SerializeField] float BaseZoom;
    [SerializeField] float BaseDuration;
    [Header("Read only")]
    public float CurrentZoom;
    public float TargetZoom;
    Coroutine currentCoroutine;
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
        zoomInfos.Add(BaseInfo);
        UpdateNewCoroutine();
    }
    public void AddZoomInfo(ZoomInfo info)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        zoomInfos.Add(info);
        UpdateNewCoroutine();
    }
    public void RemoveZoomInfo(string name)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        ZoomInfo infoToRemove = new ZoomInfo(0,0,"null");
        foreach (ZoomInfo info in zoomInfos)
        {
            if(info.Name == name)
            {
                infoToRemove = info;
            }
        }
        zoomInfos.Remove(infoToRemove);
        UpdateNewCoroutine();
    }
    ZoomInfo CheckLatestZoomInfo()
    {
        return zoomInfos.Last();
    }
    public void UpdateNewCoroutine()
    {
        ZoomInfo Latest = CheckLatestZoomInfo();
        TargetZoom = Latest.ZoomSize;
        if(CurrentZoom != TargetZoom)
        {
            currentCoroutine = StartCoroutine(ChangeZoomSmoothly(Latest));
        }
        
    }
    IEnumerator ChangeZoomSmoothly(ZoomInfo info)
    {
        float timer = 0;
        while(timer < info.ZoomDuration)
        {
            timer += Time.deltaTime;
            CurrentZoom = Mathf.Lerp(CurrentZoom, info.ZoomSize, timer / info.ZoomDuration);
            virtualCamera.m_Lens.OrthographicSize = CurrentZoom;

            yield return null;
        }
        CurrentZoom = info.ZoomSize;
    }
}
