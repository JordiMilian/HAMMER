using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullMethods : MonoBehaviour 
{
    public static string SubscribreWithArgument = "onEventWithArgument += (ArgumentType name) => MethodWithoutArgument";
    public static IEnumerator ApplyForceOverTime(Rigidbody2D rigidbody, Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            rigidbody.AddForce((forceVector / duration) * Time.deltaTime);
            yield return null;
            yield return new WaitForSeconds(.1f);
        }
    }

    //This is framerate independent now
    //The average curve value should be calculated once and stored on each script so we dont have to calculate it every time this is used
    //Test if this corotuine can be paused with TimeScale
    public static IEnumerator ApplyCurveMovementOverTime(CharacterMover2 mover, float totalDistance, float timeSeconds, Vector2 direction, AnimationCurve curve, float averageValueOfCurve = 0.5f)
    {
        float timer = 0;
        float normalizedTime;
        float normalizedTimeLastFrame = 0;
        float finalDistanceForDebug = 0;
        float evaluatedLastFrame = curve.Evaluate(0);
        while (timer < timeSeconds)
        {
            timer += Time.deltaTime* Time.timeScale;
            normalizedTime = timer / timeSeconds;
            float normalizedTimeBetweenFrames = normalizedTime - normalizedTimeLastFrame;
            float evaluatedThisFrame = curve.Evaluate(normalizedTime);
            
            float finalForce = ((evaluatedLastFrame + evaluatedThisFrame) / 2) * normalizedTimeBetweenFrames / Time.deltaTime * totalDistance / averageValueOfCurve;

            mover.MovementVectorsPerSecond.Add(direction * finalForce);

            finalDistanceForDebug += (evaluatedLastFrame + evaluatedThisFrame / 2) * normalizedTimeBetweenFrames * totalDistance * 2;

            evaluatedLastFrame = evaluatedThisFrame;
            normalizedTimeLastFrame = normalizedTime;
            yield return null;
        }

    }
    public static float GetAverageValueOfCurve(AnimationCurve curve, int samplePoints)
    {
        float evaluatePerSample = 1 / (float)samplePoints;
        float lastValue = curve.Evaluate(0);
        float totalSum = 0;
        for (int i = 0; i < samplePoints; i++)
        {
            float thisValue = curve.Evaluate(evaluatePerSample * (i + 1));
            float averageValue = (thisValue + lastValue) / 2;
            totalSum += averageValue;
            lastValue = thisValue;
        }
        return totalSum / samplePoints;
    }
    public static IEnumerator WaitForAnimationTime(AnimationClip clip)
    {
        float timer = 0;
        while (timer < clip.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    /*
    public static AnimationClip GetAnimationClipByStateName(string stateName, Animator animator, int layer = 0)
    {
        UnityEditor.Animations.AnimatorController animControl = (UnityEditor.Animations.AnimatorController)animator.runtimeAnimatorController;
        foreach (var state in animControl.layers[layer].stateMachine.states)
        {
            if (state.state.name == stateName)
            {
                return (AnimationClip)state.state.motion;
            }
        }
        return null;
    }
    */
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
    public static float AngleBetweenDirectionsDeg(Vector2 dir1, Vector2 dir2)
    {
        return Vector2.SignedAngle(dir1, dir2);
        return Mathf.Acos(Vector2.Dot(dir1.normalized, dir2.normalized)) * Mathf.Rad2Deg;
    }
    public static int simplifyScale(float scale)
    {
        if(scale >= 0) { return 1; }
        else { return -1; }
    }
    public static void CameraShakeAndHitstopFromDamage(float damage)
    {
        IntensitiesEnum intensity;
        if(damage < 0.3f) { intensity = IntensitiesEnum.VerySmall; }
        else if(damage < 0.75f) { intensity = IntensitiesEnum.Small; }
        else if(damage < 1.25) { intensity = IntensitiesEnum.Medium; }
        else if(damage < 2f) { intensity = IntensitiesEnum.Big; }
        else { intensity = IntensitiesEnum.VeryBig; }

        CameraShake.Instance.ShakeCamera(intensity);
        TimeScaleEditor.Instance.HitStop(intensity);
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
    public static Vector2[] GetPolygonPositions(Vector2 center, int sides, float radius, float offsetNormalized = 0)
    {
        List<Vector2> positions = new List<Vector2>(); 
        for (int i = 0; i < sides; i++)
        {
            float sidesF = (float)sides;
            float divider = (1.0f / sidesF * i) + offsetNormalized;

            Vector2 CurrentPoint = angle2Vector(divider * Mathf.PI * 2);
            Vector2 finalPoint = center + (CurrentPoint * radius);

            positions.Add(finalPoint);
        }

        return positions.ToArray();
    }
    public static Vector2[] GetSpreadDirectionsFromSide(Vector2 startingDirection, int amountOfPoints, float spreadRad)
    {
        List<Vector2> positions = new List<Vector2>();

        float startingAngle = vector2Angle(startingDirection);
        float angleToAdd = spreadRad/amountOfPoints;

        for (int i = 0; i < amountOfPoints; i++)
        {
            Vector2 newVector = angle2Vector(startingAngle + (angleToAdd * i));
            positions.Add(newVector);
        }

        return positions.ToArray();
    }
    public static Vector2[] GetSpreadDirectionsFromCenter(Vector2 centerDirection, int amountOfPoints, float spreadRad)
    {
        List<Vector2> positions = new List<Vector2>();

        float centerAngle = vector2Angle(centerDirection);
        float startingAngle = centerAngle - (spreadRad / 2);
        float angleToAdd = spreadRad / amountOfPoints;

        for (int i = 0; i < amountOfPoints; i++)
        {
            Vector2 newVector = angle2Vector(startingAngle + (angleToAdd * i));
            positions.Add(newVector);
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
    public static void CheckIfGameobjectImplementsInterface<T>(ref GameObject root, ref T interfaceReference ) where T : class
    {
        var component = root.GetComponent<T>();

        if (component == null)
        {
            Debug.LogWarning($"{root.name} doesn't implement {typeof(T).Name}");
            root = null;
        }
        else
        {
            interfaceReference = component;
        }
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
        Vector2 randomPoint;
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
        yield return new WaitForSecondsRealtime(delaySeconds);
        Destroy(objectToDestroy);
    }

    public static float normalizePercentage(float percent, bool negative = false, bool fromZero = false)
    {
        // Given 25%:
            // if POSITIVE return 1.25f
            // if NEGATIVE return 0.75f
            // if POSITIVE and FROMZERO return 0.25f
            // if NEGATIVE and FROMZERO return -0.25
        if(!negative)
        {
            if(fromZero)
            {
                return percent / 100;
            }
            else
            {
                return 1 + (percent / 100);
            }
        }
        else if (negative) 
        { 
            if(fromZero)
            {
                return -(percent / 100);
            }
        }
        //if (negative) and (!fromZero)
        return 1 - (percent / 100);
    }

    public static string highlightString (string text)
    {
        return "<color=red>" + text + "<color=black>";
    }

    public static float getCurrentAnimationLenght(Animator animator, int layer = 0)
    {
        return animator.GetCurrentAnimatorClipInfo(layer).Length;
    }
    public static bool IsOutsideCameraView(Vector2 worldPosition, Camera camera)
    {

        Vector2 viewportPosition = camera.WorldToViewportPoint(worldPosition);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            return true;
        }

        return false;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float time)
    {
        float timer = 0;
        float startingVolume = audioSource.volume;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(startingVolume, 0, timer / time);
            audioSource.volume = newVolume;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float time, float finalVolume)
    {
        audioSource.Play();
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(0, finalVolume, timer / time);
            audioSource.volume = newVolume;
            yield return null;
        }
        audioSource.volume = finalVolume;

    }
}
