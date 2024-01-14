using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Destroyer : MonoBehaviour
{
     float SecondsToDestroy = 3;
    void Start()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(SecondsToDestroy);
        Destroy(gameObject);
    }
}
