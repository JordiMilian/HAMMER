using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkSaw_Projectile : MonoBehaviour
{
    [SerializeField] Animator sawAnimator;
    [SerializeField] Transform sawTf;
    [SerializeField] float timeToReach;
    [SerializeField] float distance;
    public Vector2 InitialPos, FinalPos;
    public void startSawing(Vector2 initialPos, Vector2 directionToTarget)
    {
        InitialPos = initialPos;
        FinalPos = initialPos + (directionToTarget*distance);
        transform.position = InitialPos;
        
        sawAnimator.SetBool("isSawing", true);
    }
    public void EV_StartMoving()
    {
        StartCoroutine(travelToPos(InitialPos, FinalPos, timeToReach));
    }
    IEnumerator travelToPos(Vector2 initialPos, Vector2 finalPos, float timeToReach)
    {
        float timer = 0;
        float normalizedTime = 0;
        while (timer < timeToReach)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / timeToReach;

            Vector2 newPos = Vector2.Lerp(initialPos, finalPos, BezierBlend(normalizedTime));

            sawTf.position = newPos;
            yield return null;
        }
        reachedEnd();
    }
    void reachedEnd()
    {
        sawAnimator.SetTrigger("reachedEnd");
    }
    float BezierBlend(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }
}
