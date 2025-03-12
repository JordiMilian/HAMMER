using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] float lerpingZoom, targetZoom, zoomSpeed;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    Player_FollowMouseWithFocus_V2 followMouse;
    [Header("Base Zoom info")]
    [SerializeField] float BaseZoom;
    [SerializeField] float BaseSpeed;
    [Header("Focus info")]
    [SerializeField] float minZoom;
    [SerializeField] float minDistance;
    [SerializeField] float maxZoom;
    [SerializeField] float maxDistance;
    bool isFocusingZoom;
    bool checkedLatestZoom;
    Transform playerTf;

    [Serializable]
    public class ZoomInfo
    {
        public float ZoomSize;
        public float ZoomSpeed;
        public string Name;

        public ZoomInfo(float zoom, float duration, string name)
        {
            ZoomSize = zoom;
            ZoomSpeed = duration;
            Name = name;
        }
    }
    public List<ZoomInfo> zoomInfos = new List<ZoomInfo>();

    //We have a list of all the zoom infos, the lates zoom info is king. We can remove and add to the list but only listen to the last.
    //This is what happens unless the player is focusing an enemy. In which case ignore everything and focus on enemy (Player_FollowMouse will notify)


    public void SetBaseZoomAndReferences()
    {
        ZoomInfo BaseInfo = new ZoomInfo(BaseZoom, BaseSpeed, "Base");
        AddZoomInfoAndUpdate(BaseInfo);
        playerTf = GlobalPlayerReferences.Instance.playerTf;
        followMouse = GlobalPlayerReferences.Instance.references.followMouse;
        StartCoroutine(zoomUpdate());
    }
    IEnumerator zoomUpdate()
    {
        while(true)
        {
            //Lerp the zoom towards targetZoom, whatever it is. If we are focusing, we calculate the proper zoom with the focused enemy
            if (isFocusingZoom)
            {
                if (followMouse.CurrentlyFocusedIcon != null)
                {
                    targetZoom = CalculateFocusZoom(followMouse.CurrentlyFocusedIcon.transform.position);
                }
            }
            else if (!checkedLatestZoom)
            {
                GetLatestZoomInfoAndUpdateTarget();
                checkedLatestZoom = true;
            }

            lerpingZoom = Mathf.Lerp(lerpingZoom, targetZoom, Time.deltaTime * zoomSpeed);

            virtualCamera.m_Lens.OrthographicSize = lerpingZoom;

            yield return null;
        }
    }
    public void AddZoomInfoAndUpdate(ZoomInfo info)
    {
        zoomInfos.Add(info);
        checkedLatestZoom = false;
    }
    public void RemoveZoomInfoAndUpdate(string name)
    {       
        ZoomInfo infoToRemove = new ZoomInfo(0, 0, "null");
        foreach (ZoomInfo info in zoomInfos)
        {
            if (info.Name == name)
            {
                infoToRemove = info;
            }
        }
        zoomInfos.Remove(infoToRemove);

        checkedLatestZoom = false;
    }
     void GetLatestZoomInfoAndUpdateTarget()
    {
        ZoomInfo Latest = getLatestZoomInfo(); 

        zoomSpeed = Latest.ZoomSpeed;
        targetZoom = Latest.ZoomSize;
    }
    ZoomInfo getLatestZoomInfo()
    {
        if (zoomInfos.Count > 0)
        {
            return zoomInfos.Last();
        }
        else
        {
            Debug.LogWarning("What happened to BASE ZOOM INFO???");
            return null;
        }
    }
   
    public void onFocusedEnemy()
    {
        isFocusingZoom = true;
    }
    public void onUnfocusedEnemy()
    {
        isFocusingZoom = false;
        checkedLatestZoom = false;
    }
    float CalculateFocusZoom(Vector2 enemyPos) 
    {
        Vector2 playerPosition = playerTf.position;
        Vector2 enemyPosition = enemyPos;
        float distanceToEnemy = (enemyPosition - playerPosition).magnitude;

        return UsefullMethods.equivalentFromAtoB(minDistance, maxDistance, minZoom, maxZoom, distanceToEnemy);
    }
}
