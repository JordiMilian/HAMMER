using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinesRunner : MonoBehaviour
{
    public static CoroutinesRunner instance;

    private void Awake()
    {
       if(instance != null)
        {
            Destroy(gameObject);
        }
        else { instance = this; }
    }


    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
