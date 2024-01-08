using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCameraZoom : MonoBehaviour
{
   
    [SerializeField] float TargetZoom;
    [SerializeField] float ZoomSpeed = 0.003f;

    [SerializeField] CameraZoomer cameraZoomer;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        cameraZoomer.TargetZoom = TargetZoom;
        cameraZoomer.CurrentZoomSpeed = ZoomSpeed;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        cameraZoomer.TargetZoom = cameraZoomer.BaseZoom;
        cameraZoomer.CurrentZoomSpeed = cameraZoomer.BaseZoomSpeed;
    }
    
}
