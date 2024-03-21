using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullMethods
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
        if (parent.gameObject.layer == layer) { found.Add(parent.gameObject); }
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
    public static Vector2 angle2Vector(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }
    public static float vector2Angle(Vector2 vector)
    {
        Vector2 normalized = vector.normalized;
        return Mathf.Atan2(normalized.y, normalized.x);
    }
}
