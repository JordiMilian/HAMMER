using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : MonoBehaviour
{ 
    [SerializeField] float frisbeeTime;
    [SerializeField] float frisbeeDistance;

    public void throwFrisbee(Vector2 direction)
    {
        StartCoroutine(throwFrisbeeCoroutine(direction));
    }
    IEnumerator throwFrisbeeCoroutine(Vector2 direction)
    {
        float timer = 0;
        float normalizedTime = 0;
        float HalfTime = frisbeeTime / 2;

        Vector2 startingPos = transform.position;

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

        while (timer < HalfTime)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / HalfTime;

            transform.position = startingPos + ((-direction) * easeIn(normalizedTime) * frisbeeDistance);
            yield return null;
        }

        Destroy(gameObject);
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
