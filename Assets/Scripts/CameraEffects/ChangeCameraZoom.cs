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
        cameraZoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>();
        ThisInfo = new CameraZoomController.ZoomInfo(newZoom, newZoomSpeed, zoomName);
    }
    private void OnEnable()
    {
        triggerCollider.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        triggerCollider.OnTriggerEntered += addZoom;
        triggerCollider.OnTriggerExited += removeZoom;
    }
    private void OnDisable()
    {
        triggerCollider.OnTriggerEntered -= addZoom;
        triggerCollider.OnTriggerExited -= removeZoom;
    }
    void addZoom(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        cameraZoomer.AddZoomInfoAndUpdate(ThisInfo);
    }
    void removeZoom(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        cameraZoomer.RemoveZoomInfoAndUpdate(ThisInfo.Name);
    }
}
