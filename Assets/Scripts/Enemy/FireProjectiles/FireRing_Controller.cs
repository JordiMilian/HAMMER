using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing_Controller : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(UsefullMethods.destroyWithDelay(3,gameObject));  
    }
}
