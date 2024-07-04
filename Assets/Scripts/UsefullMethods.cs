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
    public static void DrawPolygon(Vector2 center, int sides, float radius, int density = 1, float offset = 0) //density is to draw satanic pentagons, keep it at 1 for normal polygons
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
    public static Vector2[] GetPolygonPositions(Vector2 center, int sides, float radius, float offset = 0)
    {
        List<Vector2> positions = new List<Vector2>(); 
        for (int i = 0; i < sides; i++)
        {
            float sidesF = intToFloat(sides);
            float divider = (1.0f / sidesF * i) + offset;

            Vector2 CurrentPoint = angle2Vector(divider * Mathf.PI * 2);
            Vector2 finalPoint = center + (CurrentPoint * radius);

            positions.Add(finalPoint);
        }

        return positions.ToArray();
    }
    public static float intToFloat(int i)
    {
        float ret = i;
        return (ret);
    }
    public static float equivalentFromAtoB(float minA, float maxA, float minB, float maxB, float initialValueA)
    {
        float normalizedA = Mathf.InverseLerp(minA, maxA, initialValueA);
        return Mathf.Lerp(minB,maxB, normalizedA);
    }
    public static void ResetAllTriggersInAnimator(Animator anim)
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
    public static void TurnAllBoolsOff(Animator anim)
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.SetBool(param.name,false);
            }
        }
    }
    public static Vector2 RandomPointInCollider(Collider2D collider)
    {
        Vector2 randomPoint = Vector2.zero;
        int attempts = 0;
        do
        {
            float x = UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x);
            float y = UnityEngine.Random.Range(collider.bounds.min.y, collider.bounds.max.y);
            randomPoint = new Vector2(x, y);
            attempts++;
        }
        while (!collider.OverlapPoint(randomPoint));
        return randomPoint;
    }
    public static BoxCollider2D BoundsToBoxCollider(Bounds bounds, Vector3 boundsOrigin, GameObject colliderHolder)
    {
        BoxCollider2D boundsCollider = colliderHolder.GetComponent<BoxCollider2D>();

        if(boundsCollider == null)
        {
            boundsCollider = colliderHolder.AddComponent<BoxCollider2D>();
            boundsCollider.isTrigger = true;
        }
        
        Vector3 localCenter = bounds.center - boundsOrigin;
        bounds.center = localCenter;

        boundsCollider.size = new Vector2(bounds.extents.x * 2, bounds.extents.y * 2);
        boundsCollider.offset = new Vector2(bounds.center.x, bounds.center.y);

        return boundsCollider;
        
    }
    public static void DrawCollider(BoxCollider2D collider, Color boxColor)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, collider.transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = boxColor;
        Gizmos.DrawWireCube(collider.offset, collider.size);
    }
    public static IEnumerator destroyWithDelay(float delaySeconds, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(delaySeconds);
        Destroy(objectToDestroy);
    }

    public static float normalizePercentage(float percent, bool negative = false)
    {
        // Given 25%:
        // if POSITIVE return 1.25f
        // if NEGATIVE return 0.75

        if (negative) { return 1 - (percent / 100); }

        return 1 + (percent / 100); 
    }

    public static string highlightString (string text)
    {
        return "<color=red>" + text + "<color=black>";
    }
}
