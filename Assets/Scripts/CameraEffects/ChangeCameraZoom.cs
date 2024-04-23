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

    CameraZoomController cameraZoomer;
    CameraZoomController.ZoomInfo ThisInfo;
    private void Awake()
    {
        cameraZoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>();
        ThisInfo = new CameraZoomController.ZoomInfo(newZoom, newZoomSpeed, zoomName);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraZoomer.AddZoomInfoAndUpdate(ThisInfo);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraZoomer.RemoveZoomInfoAndUpdate(ThisInfo.Name);
        }
           
    }
    
}
