using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCameraZoom : MonoBehaviour
{
    
    public float BaseZoom;
    public float CurrentZoom;
    public float TargetZoom;
    public float ZoomDuration;
    CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        BaseZoom = virtualCamera.m_Lens.OrthographicSize;
        CurrentZoom = BaseZoom;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CurrentZoom = virtualCamera.m_Lens.OrthographicSize;
            StartCoroutine(ChangeZoom(CurrentZoom, TargetZoom, ZoomDuration));
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { StartCoroutine(ChangeZoom(CurrentZoom, BaseZoom, ZoomDuration)); }
    }
    IEnumerator ChangeZoom(float v_start, float v_end, float duration)
    {

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            CurrentZoom = Mathf.Lerp(v_start, v_end, elapsed / duration);
            virtualCamera.m_Lens.OrthographicSize = CurrentZoom;
            elapsed += Time.deltaTime;
            yield return null;
        }
        CurrentZoom = v_end;
    }
}
