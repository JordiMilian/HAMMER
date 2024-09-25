using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    [SerializeField] GameObject CanvasRoot;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha9))
        {
            CanvasRoot.SetActive(!CanvasRoot.activeSelf);
        }
    }
}
