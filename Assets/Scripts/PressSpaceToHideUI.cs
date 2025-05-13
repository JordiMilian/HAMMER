using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSpaceToHideUI : MonoBehaviour
{
    [SerializeField] GameObject UIRoot;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIRoot.SetActive(false);
        }
    }
}
