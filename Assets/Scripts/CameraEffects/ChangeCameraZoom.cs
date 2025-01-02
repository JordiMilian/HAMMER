using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCameraZoom : MonoBehaviour
{
   
    [SerializeField] float newZoom = 5;
    [SerializeField] float newZoomSpeed = 1f;
    [SerializeField] string zoomName = "defaultZoom";
    [SerializeField] Generic_OnTriggerEnterEvents triggerCollider;

    CameraZoomController cameraZoomer;
    CameraZoomController.ZoomInfo ThisInfo;
    private void Awake()
    {
        cameraZoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        ThisInfo = new CameraZoomController.ZoomInfo(newZoom, newZoomSpeed, zoomName);
    }
    private void OnEnable()
    {
        triggerCollider.AddActivatorTag(Tags.Player_SinglePointCollider);
        triggerCollider.OnTriggerEntered += addZoom;
        triggerCollider.OnTriggerExited += removeZoom;
    }
    private void OnDisable()
    {
        triggerCollider.OnTriggerEntered -= addZoom;
        triggerCollider.OnTriggerExited -= removeZoom;
    }
    void addZoom(Collider2D collision)
    {
        cameraZoomer.AddZoomInfoAndUpdate(ThisInfo);
    }
    void removeZoom(Collider2D collision)
    {
        cameraZoomer.RemoveZoomInfoAndUpdate(ThisInfo.Name);
    }
}
