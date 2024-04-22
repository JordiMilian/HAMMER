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
    public static int simplifyScale(float scale)
    {
        if(scale >= 0) { return 1; }
        else { return -1; }
    }
    public static int normalizeFloat(float F)
    {
        if (F == 0) { return 0; }
        else if (F > 0) { return 1; }
        else { return -1; }
    }
    public static void DrawPolygon(Vector2 center, int sides, float radius, int density = 1, float offset = 0) //density is to draw satanic pentagons, keep it at 1 for normal polygon
    {
        for (int i = 0; i < sides; i++)
        {
            float sidesF = intToFloat(sides);
            float divider = (1.0f / sidesF * i) + offset;
            float previousDivider = (1.0f / sidesF * (i - density)) + offset;

            Vector2 CurrentPoint = angle2Vector(divider * Mathf.PI * 2);
            Vector2 PreviousPoint = angle2Vector(previousDivider * Mathf.PI * 2);
            Debug.DrawLine(center + (PreviousPoint * radius), center + (CurrentPoint * radius));
        }
    }
    public static float intToFloat(int i)
    {
        float ret = i;
        return (ret);
    }
    public static float equivalentFromAtoB(float minA, float maxA, float minB, float maxB, float initialValue)
    {
        float normalizedA = Mathf.InverseLerp(minA, maxA, initialValue);
        return Mathf.Lerp(minB,maxB, normalizedA);
    }
}
