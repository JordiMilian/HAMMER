using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{ 
    [SerializeField] float frisbeeTime;
    [SerializeField] float frisbeeDistance;
    public Action onReturnedFrisbee;

    public void throwFrisbee(Vector2 direction, Transform origin)
    {
        StartCoroutine(throwFrisbeeCoroutine(direction, origin));
    }
    IEnumerator throwFrisbeeCoroutine(Vector2 direction, Transform origin)
    {
        float timer = 0;
        float normalizedTime = 0;
        float HalfTime = frisbeeTime / 2;
        Vector2 originPos = origin.position;
        Vector2 startingPos = origin.position;

        while (timer < HalfTime)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / HalfTime;

            transform.position = startingPos + (direction * easeOut(normalizedTime)* frisbeeDistance);
            yield return null;
        }

        timer = 0;
        normalizedTime = 0;
        startingPos = transform.position;
        
        Vector2 ownPos = transform.position;
        Vector2 directionFromOrigin = Vector2.zero;
        float totalDistanceToOrigin = 0;


        while (timer < HalfTime)
        {
            if(origin != null) { originPos = origin.position; }
            ownPos = transform.position;
            directionFromOrigin = (ownPos - originPos).normalized;
            totalDistanceToOrigin = (startingPos - originPos).magnitude;

            timer += Time.deltaTime;
            normalizedTime = timer / HalfTime;

            float distanceThisFrame = Mathf.Lerp( totalDistanceToOrigin,0, easeIn(normalizedTime));
            Vector2 calculatedPos = originPos + (directionFromOrigin * distanceThisFrame);

            transform.position = calculatedPos;
            yield return null;
        }

        onReturnedFrisbee?.Invoke();

        StartCoroutine(UsefullMethods.destroyWithDelay(0, gameObject));
    }

    float easeOut(float normalizedInput)
    {
        return 1 - Mathf.Pow(1 - normalizedInput, 3);
    }

    float easeIn(float normalizedInput)
    {
        return Mathf.Pow(normalizedInput, 3);
    }
}
