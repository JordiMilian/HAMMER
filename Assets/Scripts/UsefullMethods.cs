using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullMethods : MonoBehaviour
{
    public static IEnumerator ApplyForceOverTime(Rigidbody2D rigidbody, Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            rigidbody.AddForce((forceVector / duration) * Time.deltaTime);
            yield return null;
        }
    }
    public static IEnumerator AddTorkeOverTime(Rigidbody2D rigidbody, Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            //rigidbody.AddTorque((forceVector / duration) * Time.deltaTime);
            yield return null;
        }
    }
    public static GameObject[] GetChildrensWithLayer (Transform parent, LayerMask layer)
    {
        List<GameObject> found = new List<GameObject>();
        for(int i = 0; i< parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if(child.gameObject.layer == layer)
            {
                found.Add(child.gameObject);
            }
        }
        return found.ToArray();
    }
}
