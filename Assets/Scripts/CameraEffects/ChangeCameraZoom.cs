using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCameraZoom : MonoBehaviour
{
   
    [SerializeField] float NewZoom;
    [SerializeField] float NewZoomDuration = 0.003f;
    [SerializeField] string Name;

    [SerializeField] CameraZoomer cameraZoomer;
    CameraZoomer.ZoomInfo ThisInfo;
    private void Awake()
    {
        cameraZoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomer>();
        ThisInfo = new CameraZoomer.ZoomInfo(NewZoom, NewZoomDuration, Name);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraZoomer.AddZoomInfo(ThisInfo);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraZoomer.RemoveZoomInfo(ThisInfo.Name);
        }
           
    }
    
}
