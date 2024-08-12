using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostUpgrade_SaveLastPosition : MonoBehaviour
{

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += SetLastDeathPosition;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= SetLastDeathPosition;
    }
    void SetLastDeathPosition()
    {
        Vector3 lastPos =  GlobalPlayerReferences.Instance.gameObject.transform.position;
        transform.position = lastPos;
    }
}
